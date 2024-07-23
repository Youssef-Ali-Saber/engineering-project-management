using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PM.Data;
using PM.Models;
using PM.Models.ViewModels;
using System.Data;
using System.Security.Claims;

namespace PM.Controllers
{
    public class InterfaceAgreementsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InterfaceAgreementsController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: InterfaceAgreements
        public async Task<IActionResult> Index(string? status, string? role, string? scopePackage)
        {


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(userId);
            var interfaceAgreements = _context.InterfaceAgreements.Include(i => i.InterfacePoint).Include(i => i.InterfacePoint.Project).ToList();


            foreach (var interfaceAgreement in interfaceAgreements)
            {
                if (interfaceAgreement.CloseDate == null && interfaceAgreement.NeedDate < DateTime.Now)
                {
                    interfaceAgreement.Status = "Overriding";
                    _context.Update(interfaceAgreement);
                }
                else if (interfaceAgreement.CloseDate == null && interfaceAgreement.IssueDate == null)
                {
                    interfaceAgreement.Status = "NotIssued";
                    _context.Update(interfaceAgreement);
                }
                else if (interfaceAgreement.CloseDate == null && DateTime.Now - DateTime.Now < interfaceAgreement.NeedDate - DateTime.Now && interfaceAgreement.NeedDate - DateTime.Now <= DateTime.Now.AddDays(30) - DateTime.Now)
                {
                    interfaceAgreement.Status = $"ClosedToDeadline";
                    _context.Update(interfaceAgreement);
                }
                else if (interfaceAgreement.CloseDate == null && interfaceAgreement.IssueDate != null)
                {
                    interfaceAgreement.Status = "InProgress";
                    _context.Update(interfaceAgreement);
                }
                else if (interfaceAgreement.CloseDate != null && interfaceAgreement.Status != "Closed")
                {
                    interfaceAgreement.Status = "Closed";
                    _context.Update(interfaceAgreement);
                }
            }

            // Apply status filter
            if (!string.IsNullOrEmpty(status))
            {
                interfaceAgreements = interfaceAgreements.Where(ia => ia.Status == status).ToList();
            }
            if (User.IsInRole("ContractorTeamMember"))
            {
                var scopePackage0 = _context.ScopePackages.FirstOrDefault(sp => sp.TeamEmails.Contains(user.Email));
                interfaceAgreements = interfaceAgreements.Where(ia => ((ia.InterfacePoint.Informed == scopePackage0.ManagerEmail || ia.InterfacePoint.Accountable == scopePackage0.ManagerEmail || ia.InterfacePoint.Consultant == scopePackage0.ManagerEmail || ia.InterfacePoint.Supported == scopePackage0.ManagerEmail || ia.InterfacePoint.Responsible == scopePackage0.ManagerEmail) && ia.IssueDate != null) || (ia.InterfacePoint.Accountable == scopePackage0.ManagerEmail)).ToList();


                if (role == "Responsible")
                    interfaceAgreements = interfaceAgreements.Where(m => m.InterfacePoint.Responsible == scopePackage0.ManagerEmail).ToList();
                else if (role == "Accountable")
                    interfaceAgreements = interfaceAgreements.Where(m => m.InterfacePoint.Accountable == scopePackage0.ManagerEmail).ToList();
                else if (role == "Consultant")
                    interfaceAgreements = interfaceAgreements.Where(m => m.InterfacePoint.Consultant == scopePackage0.ManagerEmail).ToList();
                else if (role == "Informed")
                    interfaceAgreements = interfaceAgreements.Where(m => m.InterfacePoint.Informed == scopePackage0.ManagerEmail).ToList();
                else if (role == "Supported")
                    interfaceAgreements = interfaceAgreements.Where(m => m.InterfacePoint.Supported == scopePackage0.ManagerEmail).ToList();

                ViewBag.userInterfaceAgreementsAccountable = interfaceAgreements.Where(ia => ia.InterfacePoint.Accountable == scopePackage0.ManagerEmail).ToList();
                ViewBag.AccountableCount = interfaceAgreements.Count(m => m.InterfacePoint.Accountable == scopePackage0.ManagerEmail);
                ViewBag.userInterfaceAgreementsConsultant = interfaceAgreements.Where(ia => ia.InterfacePoint.Consultant == scopePackage0.ManagerEmail).ToList();
                ViewBag.ConsultantCount = interfaceAgreements.Count(m => m.InterfacePoint.Consultant == scopePackage0.ManagerEmail);
                ViewBag.userInterfaceAgreementsSupported = interfaceAgreements.Where(ia => ia.InterfacePoint.Supported == scopePackage0.ManagerEmail).ToList();
                ViewBag.SupportedCount = interfaceAgreements.Count(m => m.InterfacePoint.Supported == scopePackage0.ManagerEmail);
                ViewBag.userInterfaceAgreementsInformed = interfaceAgreements.Where(ia => ia.InterfacePoint.Informed == scopePackage0.ManagerEmail).ToList();
                ViewBag.InformedCount = interfaceAgreements.Count(m => m.InterfacePoint.Informed == scopePackage0.ManagerEmail);
                ViewBag.userInterfaceAgreementsResponsible = interfaceAgreements.Where(ia => ia.InterfacePoint.Responsible == scopePackage0.ManagerEmail).ToList();
                ViewBag.ResponsibleCount = interfaceAgreements.Count(m => m.InterfacePoint.Responsible == scopePackage0.ManagerEmail);

            }
            else if (User.IsInRole("Contractor"))
            {

                interfaceAgreements = interfaceAgreements.Where(ia => ((ia.InterfacePoint.Informed == user.Email || ia.InterfacePoint.Accountable == user.Email || ia.InterfacePoint.Consultant == user.Email || ia.InterfacePoint.Supported == user.Email || ia.InterfacePoint.Responsible == user.Email) && ia.IssueDate != null) || (ia.InterfacePoint.Accountable == user.Email)).ToList();


                if (role == "Responsible")
                    interfaceAgreements = interfaceAgreements.Where(m => m.InterfacePoint.Responsible == user.Email).ToList();
                else if (role == "Accountable")
                    interfaceAgreements = interfaceAgreements.Where(m => m.InterfacePoint.Accountable == user.Email).ToList();
                else if (role == "Consultant")
                    interfaceAgreements = interfaceAgreements.Where(m => m.InterfacePoint.Consultant == user.Email).ToList();
                else if (role == "Informed")
                    interfaceAgreements = interfaceAgreements.Where(m => m.InterfacePoint.Informed == user.Email).ToList();
                else if (role == "Supported")
                    interfaceAgreements = interfaceAgreements.Where(m => m.InterfacePoint.Supported == user.Email).ToList();

                ViewBag.userInterfaceAgreementsConsultant = interfaceAgreements.Where(ia => ia.InterfacePoint.Consultant == user.Email).ToList();
                ViewBag.ConsultantCount = interfaceAgreements.Count(m => m.InterfacePoint.Consultant == user.Email);
                ViewBag.userInterfaceAgreementsAccountable = interfaceAgreements.Where(ia => ia.InterfacePoint.Accountable == user.Email).ToList();
                ViewBag.AccountableCount = interfaceAgreements.Count(m => m.InterfacePoint.Accountable == user.Email);
                ViewBag.userInterfaceAgreementsSupported = interfaceAgreements.Where(ia => ia.InterfacePoint.Supported == user.Email).ToList();
                ViewBag.SupportedCount = interfaceAgreements.Count(m => m.InterfacePoint.Supported == user.Email);
                ViewBag.userInterfaceAgreementsResponsible = interfaceAgreements.Where(ia => ia.InterfacePoint.Responsible == user.Email).ToList();
                ViewBag.ResponsibleCount = interfaceAgreements.Count(m => m.InterfacePoint.Responsible == user.Email);
                ViewBag.userInterfaceAgreementsInformed = interfaceAgreements.Where(ia => ia.InterfacePoint.Informed == user.Email).ToList();
                ViewBag.InformedCount = interfaceAgreements.Count(m => m.InterfacePoint.Informed == user.Email);

            }
            else if (User.IsInRole("Cordinator"))
            {
                interfaceAgreements = interfaceAgreements.Where(ia => ia.InterfacePoint.Project.OwnerId == user.Id && ia.IssueDate != null).ToList();

                ViewBag.ScopePackageList = _context.ScopePackages.Select(s => s.ManagerEmail).ToList();

                if (!string.IsNullOrEmpty(scopePackage))
                {

                    interfaceAgreements = interfaceAgreements.Where(ia => ((ia.InterfacePoint.Informed == scopePackage || ia.InterfacePoint.Accountable == scopePackage || ia.InterfacePoint.Consultant == scopePackage || ia.InterfacePoint.Supported == scopePackage || ia.InterfacePoint.Responsible == scopePackage) && ia.IssueDate != null) || (ia.InterfacePoint.Accountable == scopePackage)).ToList();


                    if (role == "Responsible")
                        interfaceAgreements = interfaceAgreements.Where(m => m.InterfacePoint.Responsible == scopePackage).ToList();
                    else if (role == "Accountable")
                        interfaceAgreements = interfaceAgreements.Where(m => m.InterfacePoint.Accountable == scopePackage).ToList();
                    else if (role == "Consultant")
                        interfaceAgreements = interfaceAgreements.Where(m => m.InterfacePoint.Consultant == scopePackage).ToList();
                    else if (role == "Informed")
                        interfaceAgreements = interfaceAgreements.Where(m => m.InterfacePoint.Informed == scopePackage).ToList();
                    else if (role == "Supported")
                        interfaceAgreements = interfaceAgreements.Where(m => m.InterfacePoint.Supported == scopePackage).ToList();

                    ViewBag.userInterfaceAgreementsConsultant = interfaceAgreements.Where(ia => ia.InterfacePoint.Consultant == scopePackage).ToList();
                    ViewBag.ConsultantCount = interfaceAgreements.Count(m => m.InterfacePoint.Consultant == scopePackage);
                    ViewBag.userInterfaceAgreementsAccountable = interfaceAgreements.Where(ia => ia.InterfacePoint.Accountable == scopePackage).ToList();
                    ViewBag.AccountableCount = interfaceAgreements.Count(m => m.InterfacePoint.Accountable == scopePackage);
                    ViewBag.userInterfaceAgreementsSupported = interfaceAgreements.Where(ia => ia.InterfacePoint.Supported == scopePackage).ToList();
                    ViewBag.SupportedCount = interfaceAgreements.Count(m => m.InterfacePoint.Supported == scopePackage);
                    ViewBag.userInterfaceAgreementsResponsible = interfaceAgreements.Where(ia => ia.InterfacePoint.Responsible == scopePackage).ToList();
                    ViewBag.ResponsibleCount = interfaceAgreements.Count(m => m.InterfacePoint.Responsible == scopePackage);
                    ViewBag.userInterfaceAgreementsInformed = interfaceAgreements.Where(ia => ia.InterfacePoint.Informed == scopePackage).ToList();
                    ViewBag.InformedCount = interfaceAgreements.Count(m => m.InterfacePoint.Informed == scopePackage);

                }
            }
            var statusCounts = interfaceAgreements.GroupBy(ia => ia.Status).ToDictionary(g => g.Key, g => g.Count());
            ViewBag.StatusCounts = statusCounts;

            return View(interfaceAgreements);
        }


        // GET: InterfaceAgreements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var interfaceAgreement = await _context.InterfaceAgreements
                .Include(i => i.InterfacePoint)
                .Include(i => i.InterfacePoint.Project)
                .Include(i => i.InterfacePoint.Project.ScopePackages)
                .Include(i => i.Documentations)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (interfaceAgreement == null)
            {
                return NotFound();
            }



            return View(interfaceAgreement);
        }

        // GET: InterfaceAgreements/Create
        public IActionResult Create(int Id)
        {
            var viewModel = new InterfaceAgreementViewModel();
            viewModel.InterfacePointId = Id;
            var project = _context.InterfacePoints.Include(ip => ip.Project).Include(p => p.Project.Systems).Include(p => p.Project.ScopePackages).FirstOrDefault(ip => ip.Id == Id)?.Project;
            if (project?.Systems != null)
                ViewBag.Systems = project.Systems.Select(sp => new SelectListItem { Text = sp.Name, Value = sp.Name }).ToList();
            if (project?.ScopePackages != null)
                ViewBag.ScopePackages = project.ScopePackages.Select(sp => new SelectListItem { Text = sp.Name, Value = sp.ManagerEmail }).ToList();
            return View(viewModel);
        }

        // POST: InterfaceAgreements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InterfaceAgreementViewModel viewModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(userId);
            var interfacePoint = await _context.InterfacePoints.FindAsync(viewModel.InterfacePointId);
            var interfaceAgreement = new InterfaceAgreement
            {
                AccountableTeamMemberEmail = user?.Email,
                CreatedDate = DateTime.Now,
                Description = viewModel.Description,
                Title = viewModel.Title,
                InterfacePointId = viewModel.InterfacePointId,
                System = viewModel.System,
                Status = "InProgress",
                NeedDate = viewModel.NeedDate,
                Discipline=viewModel.Discipline
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
                    interfaceAgreement.Documentations.Add(documentation);
                }
            }

            _context.Add(interfaceAgreement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: InterfaceAgreements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interfaceAgreement = await _context.InterfaceAgreements.FindAsync(id);
            if (interfaceAgreement == null)
            {
                return NotFound();
            }
            var interfaceAgreementViewModel = new InterfaceAgreementViewModel
            {
                Description = interfaceAgreement.Description,
                Title = interfaceAgreement.Title,
                System = interfaceAgreement.System,
                NeedDate = interfaceAgreement.NeedDate,
                InterfacePointId = interfaceAgreement.InterfacePointId,
                Id = interfaceAgreement.Id,
                Discipline=interfaceAgreement.Discipline
            };

            var project = _context.InterfacePoints.Include(ip => ip.Project).Include(p => p.Project.Systems).Include(p => p.Project.ScopePackages).FirstOrDefault(ip => ip.Id == interfaceAgreement.InterfacePointId)?.Project;
            if (project?.Systems != null)
                ViewBag.Systems = project.Systems.Select(sp => new SelectListItem { Text = sp.Name, Value = sp.Name }).ToList();
            if (project?.ScopePackages != null)
                ViewBag.ScopePackages = project.ScopePackages.Select(sp => new SelectListItem { Text = sp.Name, Value = sp.ManagerEmail }).ToList();
            return View(interfaceAgreementViewModel);
        }

        // POST: InterfaceAgreements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(InterfaceAgreementViewModel viewModel)
        {
            try
            {
                var interfaceAgreement = await _context.InterfaceAgreements.Include(ia => ia.Documentations).FirstOrDefaultAsync(ia => ia.Id == viewModel.Id);
                if (interfaceAgreement == null)
                {
                    return NotFound();
                }
                interfaceAgreement.Description = viewModel.Description;
                interfaceAgreement.Title = viewModel.Title;
                interfaceAgreement.System = viewModel.System;
                interfaceAgreement.NeedDate = viewModel.NeedDate;
                interfaceAgreement.Discipline = viewModel.Discipline;

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
                        interfaceAgreement.Documentations.Add(documentation);
                    }
                }
                _context.InterfaceAgreements.Update(interfaceAgreement);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InterfaceAgreementExists(viewModel.Id))
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


        // POST: InterfaceAgreements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var interfaceAgreement = await _context.InterfaceAgreements.FirstOrDefaultAsync(ia => ia.Id == id);
            if (interfaceAgreement != null)
            {
                _context.InterfaceAgreements.Remove(interfaceAgreement);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Close(int id)
        {
            var interfaceAgreement = await _context.InterfaceAgreements.Include(i => i.InterfacePoint).FirstOrDefaultAsync(ia => ia.Id == id);
            bool isAllClosed = true;
            if (interfaceAgreement == null)
            {
                return NotFound();
            }

            interfaceAgreement.CloseDate = DateTime.Now;
            interfaceAgreement.Status = "Closed";
            foreach (var interfaceAgreement0 in _context.InterfaceAgreements.Where(ip => ip.InterfacePointId == interfaceAgreement.InterfacePoint.Id).ToList())
            {
                if (interfaceAgreement0.CloseDate == null)
                    isAllClosed = false;
            }
            if (isAllClosed)
            {
                interfaceAgreement.InterfacePoint.CloseDate = DateTime.Now;
                interfaceAgreement.InterfacePoint.Status = "Closed";
                _context.Update(interfaceAgreement.InterfacePoint);
            }
            _context.Update(interfaceAgreement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id });
        }
        public async Task<IActionResult> Issue(int id)
        {
            var interfaceAgreement = await _context.InterfaceAgreements.FindAsync(id);
            if (interfaceAgreement == null)
            {
                return NotFound();
            }

            interfaceAgreement.IssueDate = DateTime.Now;
            interfaceAgreement.Status = "InProgress";
            _context.Update(interfaceAgreement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id });
        }


        private bool InterfaceAgreementExists(int id)
        {
            return _context.InterfaceAgreements.Any(e => e.Id == id);
        }
    }
}
