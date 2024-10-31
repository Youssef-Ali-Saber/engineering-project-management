using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PM.Data;
using PM.Models;
using PM.Models.ViewModels;
using PM.Services;
using System.Security.Claims;

namespace PM.Controllers
{
    [Authorize]
    public class InterfacePointsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly NotificationService _notificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public InterfacePointsController(ApplicationDbContext context, NotificationService notificationService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _notificationService = notificationService;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: InterfacePoints
        public async Task<IActionResult> Index(string? scope, string? nature, string? category, string? role, string? scopePackage, string? boq, string? activity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(userId);
            var interfacePoints = _context.InterfacePoints
                .Include(i => i.Project)
                .ThenInclude(c => c.ScopePackages)
                .Include(interfacePoint => interfacePoint.BOQs).Include(interfacePoint => interfacePoint.Activities).ToList();

            var projects = _context.Projects
                .Include(inc => inc.ScopePackages)
                .Include(inc => inc.Departments)
                .Include(project => project.BOQs)
                .Include(project => project.Activities)
                .Where(m => m.Departments.Any(m => m.TeamMembersEmails.Any(tm => tm == user.Email))
                            || m.Departments.Any(m => m.TeamManagerEmail == user.Email)
                            || m.ScopePackages.Any(c => c.ManagerEmail == user.Email)
                            || m.ScopePackages.Any(v => v.TeamEmails.Contains(user.Email))
                            || m.OwnerId == user.Id).ToList();

            ViewBag.BOQList = projects.SelectMany(c => c.BOQs).Select(s => s.Name).ToList();
            ViewBag.ActivityList = projects.SelectMany(c => c.Activities).Select(s => s.Name).ToList();


            // Filters
            if (scope != null)
                interfacePoints = interfacePoints.Where(m => m.Scope == scope).ToList();
            if (nature != null)
                interfacePoints = interfacePoints.Where(m => m.Nature == nature).ToList();
            if (category != null)
                interfacePoints = interfacePoints.Where(m => m.Category == category).ToList();
            if (!string.IsNullOrEmpty(scopePackage))
            {
                interfacePoints = interfacePoints.Where(m => scopePackage == m.Accountable || scopePackage == m.Supported || scopePackage == m.Informed || scopePackage == m.Consultant || scopePackage == m.Responsible).ToList();

                if (role != null)
                {

                    if (role == "Responsible")
                        interfacePoints = interfacePoints.Where(m => m.Responsible == scopePackage).ToList();
                    else if (role == "Accountable")
                        interfacePoints = interfacePoints.Where(m => m.Accountable == scopePackage).ToList();
                    else if (role == "Consultant")
                        interfacePoints = interfacePoints.Where(m => m.Consultant == scopePackage).ToList();
                    else if (role == "Informed")
                        interfacePoints = interfacePoints.Where(m => m.Informed == scopePackage).ToList();
                    else if (role == "Supported")
                        interfacePoints = interfacePoints.Where(m => m.Supported == scopePackage).ToList();
                }

                ViewBag.InterfacePointsAccountable = interfacePoints.Where(m => m.Accountable == scopePackage).ToList();
                ViewBag.InterfacePointsResponsible = interfacePoints.Where(m => m.Responsible == scopePackage).ToList();
                ViewBag.InterfacePointsConsultant = interfacePoints.Where(m => m.Consultant == scopePackage).ToList();
                ViewBag.InterfacePointsInformed = interfacePoints.Where(m => m.Informed == scopePackage).ToList();
                ViewBag.InterfacePointsSupported = interfacePoints.Where(m => m.Supported == scopePackage).ToList();

                ViewBag.Responsible = interfacePoints.Count(i => i.Responsible == scopePackage);
                ViewBag.Accountable = interfacePoints.Count(i => i.Accountable == scopePackage);
                ViewBag.Consultant = interfacePoints.Count(i => i.Consultant == scopePackage);
                ViewBag.Informed = interfacePoints.Count(i => i.Informed == scopePackage);
                ViewBag.Supported = interfacePoints.Count(i => i.Supported == scopePackage);

            }
            if (!string.IsNullOrEmpty(boq))
                interfacePoints = interfacePoints.Where(m => m.BOQs.Any(b => b.Name == boq)).ToList();
            if (!string.IsNullOrEmpty(activity))
                interfacePoints = interfacePoints.Where(m => m.Activities.Any(v => v.Name == activity)).ToList();



            // User-specific filtering logic
            if (User.IsInRole("Cordinator"))
            {
                interfacePoints = interfacePoints.Where(m => m.Project.OwnerId == userId && m.Status != "Holding").ToList();

            }
            else if (User.IsInRole("TeamManager"))
            {
                var departments = _context.Departments.Where(x => x.TeamManagerEmail == user.Email).ToList();

                var members = _context.Users.AsEnumerable().Where(x => departments.Any(c => c.TeamMembersEmails.Contains(x.Email))).ToList();

                interfacePoints = interfacePoints.Where(m =>
                    m.CreatorId == user.Id ||
                    members.Select(x => x.Id).Contains(m.CreatorId) ||
                    m.DepIds.Any(depId => departments.Select(d => d.Id).Contains(depId))
                ).ToList();

                ViewBag.ourInterfacePoints = interfacePoints.Where(m =>
                    m.CreatorId == user.Id ||
                    members.Select(x => x.Id).Contains(m.CreatorId) ||
                    m.DepIds.Any(depId => departments.Select(d => d.Id).Contains(depId))
                ).ToList();
            }
            else if (User.IsInRole("TeamMember"))
            {
                ViewBag.Projects = _context.Projects.Include(x => x.Departments).Where(m =>
                        m.Departments.Any(m => m.TeamMembersEmails.Any(tm => tm == user.Email)))
                    .Select(c => new SelectListItem(c.ProjectName, c.Id.ToString()));

                var departments = _context.Departments.Where(x => x.TeamMembersEmails.Contains(user.Email)).ToList();

                var members = _context.Users.AsEnumerable().Where(x => departments.Any(c => c.TeamMembersEmails.Contains(x.Email)))
                    .ToList();
                var manager = _context.Users.AsEnumerable().FirstOrDefault(x => departments.Any(v => v.TeamManagerEmail == x.Email));


                interfacePoints = interfacePoints.Where(m =>
                    ((m.CreatorId == user.Id || members.Select(x => x.Id).Contains(m.CreatorId)) || m.CreatorId == manager.Email || m.DepIds.Any(depId => departments.Select(d => d.Id).Contains(depId)))).ToList();
            }
            else if (User.IsInRole("Contractor"))
            {
                var scopePackage0 = _context.ScopePackages.Include(inc => inc.Project).FirstOrDefault(m => m.ManagerEmail == user.Email);

                var members = _context.Users.Where(u => scopePackage0.TeamEmails.Contains(u.Email)).ToList();

                interfacePoints = interfacePoints.Where(m => ((m.Responsible == user.Email || m.Accountable == user.Email || m.Consultant == user.Email || m.Informed == user.Email || m.Supported == user.Email) && m.IssueDate != null) || m.CreatorId == userId || members.Select(x => x.Id).Contains(m.CreatorId)).ToList();

                ViewBag.ourInterfacePoints = interfacePoints.Where(m => m.CreatorId == userId || members.Select(x => x.Id).Contains(m.CreatorId)).ToList();


                ViewBag.Projects = _context.Projects.Include(x => x.ScopePackages).Where(m =>
                        m.ScopePackages.Any(sp => sp.ManagerEmail == user.Email))
                    .Select(c => new SelectListItem(c.ProjectName, c.Id.ToString()));

                if (role == "Responsible")
                    interfacePoints = interfacePoints.Where(m => m.Responsible == user.Email).ToList();
                else if (role == "Accountable")
                    interfacePoints = interfacePoints.Where(m => m.Accountable == user.Email).ToList();
                else if (role == "Consultant")
                    interfacePoints = interfacePoints.Where(m => m.Consultant == user.Email).ToList();
                else if (role == "Informed")
                    interfacePoints = interfacePoints.Where(m => m.Informed == user.Email).ToList();
                else if (role == "Supported")
                    interfacePoints = interfacePoints.Where(m => m.Supported == user.Email).ToList();


                ViewBag.Responsible = interfacePoints.Count(i => i.Responsible == user.Email);
                ViewBag.Accountable = interfacePoints.Count(i => i.Accountable == user.Email);
                ViewBag.Consultant = interfacePoints.Count(i => i.Consultant == user.Email);
                ViewBag.Informed = interfacePoints.Count(i => i.Informed == user.Email);
                ViewBag.Supported = interfacePoints.Count(i => i.Supported == user.Email);

                ViewBag.InterfacePointsAccountable = interfacePoints.Where(m => m.Accountable == user.Email).ToList();
                ViewBag.InterfacePointsResponsible = interfacePoints.Where(m => m.Responsible == user.Email).ToList();
                ViewBag.InterfacePointsConsultant = interfacePoints.Where(m => m.Consultant == user.Email).ToList();
                ViewBag.InterfacePointsInformed = interfacePoints.Where(m => m.Informed == user.Email).ToList();
                ViewBag.InterfacePointsSupported = interfacePoints.Where(m => m.Supported == user.Email).ToList();

            }
            else if (User.IsInRole("ContractorTeamMember"))
            {
                var scopePackage0 = _context.ScopePackages.Include(inc => inc.Project).Where(m => m.TeamEmails.Contains(user.Email));

                var projectIds = scopePackage0.Select(x => x.Project.Id).ToList();

                var manager = _context.Users.FirstOrDefault(u => scopePackage0.Any(x => x.ManagerEmail == u.Email));

                var members = _context.Users.Where(u => scopePackage0.Any(x => x.TeamEmails.Contains(u.Email))).ToList();

                ViewBag.Projects = _context.Projects.Include(x => x.ScopePackages).Where(m =>
                    m.ScopePackages.Any(sp => sp.TeamEmails.Contains(user.Email)))
                    .Select(c => new SelectListItem(c.ProjectName, c.Id.ToString()));

                if (scopePackage0 == null)
                {
                    return NotFound();
                }

                interfacePoints = interfacePoints.Where(m => (((scopePackage0.Any(x => x.ManagerEmail == m.Responsible) ||
                                                                scopePackage0.Any(x => x.ManagerEmail == m.Accountable) ||
                                                                scopePackage0.Any(x => x.ManagerEmail == m.Consultant) ||
                                                                scopePackage0.Any(x => x.ManagerEmail == m.Informed) ||
                                                                scopePackage0.Any(x => x.ManagerEmail == m.Supported)) && m.IssueDate != null) ||
                                                              m.CreatorId == manager.Id || m.CreatorId == userId || members.Select(x => x.Id).Contains(m.CreatorId)) && projectIds.Contains(m.ProjectId)).ToList();

                ViewBag.ourInterfacePoints = interfacePoints.Where(m => m.CreatorId == manager.Id || m.CreatorId == userId || members.Select(x => x.Id).Contains(m.CreatorId)).ToList();

                if (role == "Responsible")
                    interfacePoints = interfacePoints.Where(m => scopePackage0.Any(x => x.ManagerEmail == m.Responsible)).ToList();
                else if (role == "Accountable")
                    interfacePoints = interfacePoints.Where(m => scopePackage0.Any(x => x.ManagerEmail == m.Accountable)).ToList();
                else if (role == "Consultant")
                    interfacePoints = interfacePoints.Where(m => scopePackage0.Any(x => x.ManagerEmail == m.Consultant)).ToList();
                else if (role == "Informed")
                    interfacePoints = interfacePoints.Where(m => scopePackage0.Any(x => x.ManagerEmail == m.Informed)).ToList();
                else if (role == "Supported")
                    interfacePoints = interfacePoints.Where(m => scopePackage0.Any(x => x.ManagerEmail == m.Supported)).ToList();

                ViewBag.InterfacePointsAccountable = interfacePoints.Where(m => scopePackage0.Any(x => x.ManagerEmail == m.Accountable)).ToList();
                ViewBag.InterfacePointsResponsible = interfacePoints.Where(m => scopePackage0.Any(x => x.ManagerEmail == m.Responsible)).ToList();
                ViewBag.InterfacePointsConsultant = interfacePoints.Where(m => scopePackage0.Any(x => x.ManagerEmail == m.Consultant)).ToList();
                ViewBag.InterfacePointsInformed = interfacePoints.Where(m => scopePackage0.Any(x => x.ManagerEmail == m.Informed)).ToList();
                ViewBag.InterfacePointsSupported = interfacePoints.Where(m => scopePackage0.Any(x => x.ManagerEmail == m.Supported)).ToList();


                ViewBag.Accountable = interfacePoints.Count(i => scopePackage0.Any(x => x.ManagerEmail == i.Accountable));
                ViewBag.Responsible = interfacePoints.Count(i => scopePackage0.Any(x => x.ManagerEmail == i.Responsible));
                ViewBag.Consultant = interfacePoints.Count(i => scopePackage0.Any(x => x.ManagerEmail == i.Consultant));
                ViewBag.Informed = interfacePoints.Count(i => scopePackage0.Any(x => x.ManagerEmail == i.Informed));
                ViewBag.Supported = interfacePoints.Count(i => scopePackage0.Any(x => x.ManagerEmail == i.Supported));
            }

            var scopeCounts = interfacePoints.GroupBy(i => i.Scope).ToDictionary(g => g.Key, g => g.Count());
            var natureCounts = interfacePoints.GroupBy(i => i.Nature).ToDictionary(g => g.Key, g => g.Count());
            var categoryCounts = interfacePoints.GroupBy(i => i.Category).ToDictionary(g => g.Key, g => g.Count());

            var boqCounts = interfacePoints.SelectMany(x => x.BOQs).GroupBy(i => i.Name).ToDictionary(g => g.Key, g => g.Count());
            var activityCounts = interfacePoints.SelectMany(x => x.Activities).GroupBy(i => i.Name).ToDictionary(g => g.Key, g => g.Count());

            ViewBag.ScopeCounts = scopeCounts;
            ViewBag.NatureCounts = natureCounts;
            ViewBag.CategoryCounts = categoryCounts;

            ViewBag.ActivityCounts = activityCounts;
            ViewBag.BoqCounts = boqCounts;

            var projectList = _context.Projects
                .Include(x => x.ScopePackages)
                .Where(m => m.OwnerId == user.Id).ToList();

            List<string> scopePackages = [];

            ViewBag.ScopePackageList = projectList.Aggregate(scopePackages, (current, project) => current.Union(project.ScopePackages.Select(sp => sp.ManagerEmail).ToList()).ToList());

            return View(interfacePoints);
        }


        // GET: InterfacePoints/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interfacePoint = await _context.InterfacePoints
                .Include(i => i.Project).Include(i => i.Project.Departments)
                .Include(i => i.BOQs)
                .Include(i => i.Activities)
                .Include(i => i.Documentations)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (interfacePoint == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.Include(inc => inc.Departments).FirstOrDefaultAsync(f => f.Id == interfacePoint.ProjectId);

            var dep = project.Departments.Where(i => !interfacePoint.DepIds.Contains(i.Id)).Select(x => new SelectListItem(text: x.Name, value: x.Id.ToString())).ToList();

            if (dep.Count > 0)
                ViewBag.dep = dep;

            return View(interfacePoint);
        }

        // GET: InterfacePoints/Create
        public IActionResult Create(int projectId)
        {

            var project = _context.Projects.Include(inc => inc.ScopePackages).Include(inc => inc.BOQs).Include(inc => inc.Activities).Include(i => i.Systems)
                .Include(i => i.Departments).FirstOrDefault(m => m.Id == projectId);


            if (project == null)
            {
                return NotFound("Not Found Project");
            }

            if (project.ScopePackages != null)
                ViewBag.ScopePackages = project.ScopePackages.Select(sp => new SelectListItem { Text = sp.Name, Value = sp.ManagerEmail }).ToList();
            if (project.Systems != null)
                ViewBag.Systems = project.Systems.Select(sp => new SelectListItem { Text = sp.Name, Value = sp.Name }).ToList();
            if (project.BOQs != null)
                ViewBag.BOQs = project.BOQs.Select(sp => new SelectListItem { Text = sp.Name, Value = sp.Name }).ToList();
            if (project.Activities != null)
                ViewBag.Activities = project.Activities.Select(sp => new SelectListItem { Text = sp.Name, Value = sp.Name }).ToList();

            ViewBag.projectId = project.Id;

            return View();

        }

        // POST: InterfacePoints/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InterFacePointViewModel viewModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var project = _context.Projects.Include(inc => inc.ScopePackages).Include(inc => inc.BOQs).Include(inc => inc.Activities).Include(inc => inc.Departments).FirstOrDefault(m => m.Id == viewModel.ProjectId);


            var interfacePoint = new InterfacePoint
            {
                Nature = viewModel.Nature,
                Scope = viewModel.Scope,
                ScopePackage1 = viewModel.ScopePackage1,
                ScopePackage2 = viewModel.ScopePackage2,
                System1 = viewModel.System1,
                System2 = viewModel.System2,
                ExtraSystem = viewModel.ExtraSystem,
                Category = viewModel.Category,
                Responsible = viewModel.Responsible,
                Consultant = viewModel.Consultant,
                Accountable = viewModel.Accountable,
                Informed = viewModel.Informed,
                Supported = viewModel.Supported,
                CreatDate = DateTime.Now,
                ProjectId = project.Id,
                Description = viewModel.Description,
                Status = "Holding",
                CreatorId = userId,
            };

            if (viewModel.Documentations != null)
            {
                foreach (var documentation in viewModel.Documentations)
                {
                    if (documentation.DocumentationFile != null)
                    {
                        var fnameGuid = Guid.NewGuid();
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fnameGuid + documentation.DocumentationFile.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await documentation.DocumentationFile.CopyToAsync(stream);
                        }
                        documentation.DocumentationLink = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/uploads/{fnameGuid}{documentation.DocumentationFile.FileName}";
                    }
                    interfacePoint.Documentations.Add(documentation);
                }
            }
            if (viewModel.BOQs != null)
            {
                foreach (var boq in viewModel.BOQs)
                {
                    var BOQTable = project.BOQs.FirstOrDefault(m => m.Name == boq);
                    if (BOQTable != null)
                        interfacePoint.BOQs.Add(BOQTable);
                }
            }
            if (viewModel.Activities != null)
            {
                foreach (var activity in viewModel.Activities)
                {
                    var activityTable = project.Activities.FirstOrDefault(m => m.Name == activity);
                    if (activityTable != null)
                        interfacePoint.Activities.Add(activityTable);
                }
            }

            _context.Add(interfacePoint);
            await _context.SaveChangesAsync();

            var allUsers = _context.Users.ToList();


            // Filter users based on the specified conditions
            var usersToNotify = allUsers.Where(m =>
                m.Id == project.OwnerId ||
                project.Departments.Any(n => n.TeamMembersEmails.Contains(m.Email) || n.TeamManagerEmail == m.Email) ||
                project.ScopePackages.Any(a => a.ManagerEmail == m.Email)).ToList();

            var department = _context.Departments.FirstOrDefault(n => n.TeamMembersEmails.Contains(user.Email));
            if (department != null)
                _notificationService.CreateNotification($"{user.FullName} From Department {department.Name} Created New Interface Point IP_{interfacePoint.Id}", usersToNotify);

            var scopePackage = project.ScopePackages.FirstOrDefault(n => n.ManagerEmail == user.Email);
            if (scopePackage != null)
                _notificationService.CreateNotification($"{user.FullName} From Scope Package {scopePackage.Name} Created New Interface Point IP_{interfacePoint.Id}", usersToNotify);
            return RedirectToAction(nameof(Index));
        }

        // GET: InterfacePoints/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interfacePoint = await _context.InterfacePoints
                .FirstOrDefaultAsync(ip => ip.Id == id);

            if (interfacePoint == null)
            {
                return NotFound();
            }

            var interfacePointViewModel = new InterFacePointViewModel
            {
                Nature = interfacePoint.Nature,
                Accountable = interfacePoint.Accountable,
                Category = interfacePoint.Category,
                Consultant = interfacePoint.Consultant,
                ExtraSystem = interfacePoint.ExtraSystem,
                Id = interfacePoint.Id,
                Informed = interfacePoint.Informed,
                Responsible = interfacePoint.Responsible,
                Scope = interfacePoint.Scope,
                ScopePackage1 = interfacePoint.ScopePackage1,
                ScopePackage2 = interfacePoint.ScopePackage2,
                System1 = interfacePoint.System1,
                System2 = interfacePoint.System2,
                Supported = interfacePoint.Supported,
                Description = interfacePoint.Description,
            };

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var project = _context.Projects.Include(inc => inc.ScopePackages).Include(inc => inc.BOQs).Include(inc => inc.Activities).Include(i => i.Systems)
                .Include(i => i.Departments).FirstOrDefault(m => m.Id == interfacePoint.ProjectId);

            if (project != null)
            {
                if (project.ScopePackages != null)
                    ViewBag.ScopePackages = project.ScopePackages.Select(sp => new SelectListItem { Text = sp.Name, Value = sp.ManagerEmail }).ToList();
                if (project.Systems != null)
                    ViewBag.Systems = project.Systems.Select(sp => new SelectListItem { Text = sp.Name, Value = sp.Name }).ToList();
                if (project.BOQs != null)
                    ViewBag.BOQs = project.BOQs.Select(sp => new SelectListItem { Text = sp.Name, Value = sp.Name }).ToList();
                if (project.Activities != null)
                    ViewBag.Activities = project.Activities.Select(sp => new SelectListItem { Text = sp.Name, Value = sp.Name }).ToList();

                return View(interfacePointViewModel);
            }

            return View(interfacePointViewModel);
        }

        // POST: InterfacePoints/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, InterFacePointViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);

                var interfacePoint = _context.InterfacePoints
                    .Include(m => m.Documentations)
                    .Include(ip => ip.BOQs)
                    .Include(ip => ip.Activities)
                    .FirstOrDefault(m => m.Id == id);

                var project = _context.Projects.Include(inc => inc.Departments).Include(inc => inc.ScopePackages).FirstOrDefault(m => m.Id == interfacePoint.ProjectId);

                interfacePoint.Nature = viewModel.Nature;
                interfacePoint.Scope = viewModel.Scope;
                interfacePoint.ScopePackage1 = viewModel.ScopePackage1;
                interfacePoint.ScopePackage2 = viewModel.ScopePackage2;
                interfacePoint.System1 = viewModel.System1;
                interfacePoint.System2 = viewModel.System2;
                interfacePoint.ExtraSystem = viewModel.ExtraSystem;
                interfacePoint.Category = viewModel.Category;
                interfacePoint.Responsible = viewModel.Responsible;
                interfacePoint.Consultant = viewModel.Consultant;
                interfacePoint.Accountable = viewModel.Accountable;
                interfacePoint.Informed = viewModel.Informed;
                interfacePoint.Supported = viewModel.Supported;
                interfacePoint.ProjectId = project.Id;
                interfacePoint.Description = viewModel.Description;

                interfacePoint.IssueDate = null;
                interfacePoint.Status = "Holding";

                if (viewModel.Documentations != null)
                {
                    foreach (var documentation in viewModel.Documentations)
                    {
                        if (documentation.DocumentationFile != null)
                        {

                            var fnameGuid = Guid.NewGuid();
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fnameGuid + documentation.DocumentationFile.FileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await documentation.DocumentationFile.CopyToAsync(stream);
                            }
                            documentation.DocumentationLink = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/uploads/{fnameGuid}{documentation.DocumentationFile.FileName}";
                        }
                        interfacePoint.Documentations.Add(documentation);
                    }
                }

                interfacePoint.BOQs = [];
                foreach (var boq in viewModel.BOQs ?? [])
                {
                    var BOQTable = _context.BOQs.FirstOrDefault(m => m.Name == boq && m.ProjectId == project.Id);
                    if (BOQTable != null)
                        interfacePoint.BOQs.Add(BOQTable);
                }

                interfacePoint.Activities = [];
                foreach (var activity in viewModel.Activities ?? [])
                {
                    var activityTable = _context.Activities.FirstOrDefault(m => m.Name == activity && m.ProjectId == project.Id);
                    if (activityTable != null)
                        interfacePoint.Activities.Add(activityTable);
                }


                _context.Update(interfacePoint);
                await _context.SaveChangesAsync();

                var allUsers = _context.Users.ToList();


                var usersToNotify = allUsers.Where(m =>
                    m.Id == project.OwnerId ||
                    project.Departments.Any(n => n.TeamMembersEmails.Contains(m.Email) || n.TeamManagerEmail == m.Email) ||
                    project.ScopePackages.Any(a => a.ManagerEmail == m.Email)).ToList();

                if (User.IsInRole("TeamMember"))
                {
                    _notificationService.CreateNotification($"{user.FullName} From Department {_context.Departments.FirstOrDefault(n => n.TeamMembersEmails.Contains(user.Email)).Name} Edited Interface Point IP_{interfacePoint.Id}", usersToNotify);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InterfacePointExists(viewModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));

        }


        // POST: InterfacePoints/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var interfacePoint = await _context.InterfacePoints.FirstOrDefaultAsync(f => f.Id == id);

            if (interfacePoint == null)
            {
                return NotFound();
            }

            _context.InterfacePoints.Remove(interfacePoint);
            await _context.SaveChangesAsync();

            var project = await _context.Projects
                .Include(inc => inc.Departments)
                .Include(inc => inc.ScopePackages)
                .FirstOrDefaultAsync(f => f.Id == interfacePoint.ProjectId);

            if (project == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var allUsers = await _context.Users.ToListAsync();

            var usersToNotify = allUsers.Where(m =>
                m.Id == project.OwnerId ||
                project.Departments.Any(n => n.TeamMembersEmails.Contains(m.Email) || n.TeamManagerEmail == m.Email) ||
                project.ScopePackages.Any(a => a.ManagerEmail == m.Email)).ToList();

            var userDepartment = _context.Departments.FirstOrDefault(n => n.TeamMembersEmails.Contains(user.Email));

            if (userDepartment != null)
            {
                _notificationService.CreateNotification($"{user.FullName} from Department {userDepartment.Name} deleted Interface Point IP_{interfacePoint.Id}", usersToNotify);
            }
            else
            {
                _notificationService.CreateNotification($"{user.FullName} deleted Interface Point IP_{interfacePoint.Id}", usersToNotify);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool InterfacePointExists(int id)
        {
            return _context.InterfacePoints.Any(e => e.Id == id);
        }


        public async Task<IActionResult> Issue(int id)
        {
            var interfacePoint = await _context.InterfacePoints.FindAsync(id);
            if (interfacePoint == null)
            {
                return NotFound();
            }

            if (User.IsInRole("Cordinator"))
            {
                interfacePoint.IssueDate = DateTime.Now;
                interfacePoint.Status = "InProgress";
            }
            else
            {
                interfacePoint.Status = "Pending";
            }
            _context.Update(interfacePoint);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id });
        }

        public async Task<IActionResult> SendToDep(int id, int depId)
        {
            var interfacePoint = await _context.InterfacePoints.FindAsync(id);
            if (interfacePoint == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(inc => inc.Departments)
                .Include(inc => inc.ScopePackages)
                .FirstOrDefaultAsync(f => f.Id == interfacePoint.ProjectId);

            var dep = project.Departments.FirstOrDefault(f => f.Id == depId);

            if (dep == null)
            {
                return NotFound();
            }

            interfacePoint.DepIds.Add(depId);

            interfacePoint.DepIds = interfacePoint.DepIds.ToHashSet().ToList();

            _context.Update(interfacePoint);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id, string status)
        {
            var interfacePoint = await _context.InterfacePoints.FindAsync(id);
            if (interfacePoint == null)
            {
                return NotFound();
            }

            interfacePoint.Status = status;
            _context.Update(interfacePoint);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
