using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PM.Data;
using PM.Models;
using PM.Models.ViewModels;

namespace PM.Controllers
{
    [Authorize]
    public class InterfacePointsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InterfacePointsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: InterfacePoints
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(userId);
            var interfacePoints = _context.InterfacePoints;
            if (User.IsInRole("Cordinator"))
                return View(await interfacePoints.Include(i => i.Project).Where(m => m.Project.OwnerId == userId).ToListAsync());
            
            else if(User.IsInRole("TeamManager")|| User.IsInRole("TeamMember"))
            {
                var project = _context.Projects.Include(inc => inc.ScopePackages).Include(inc => inc.Departments).FirstOrDefault(m => m.Departments.Any(m => m.TeamMembersEmails.Any(tm => tm == user.Email)) || m.Departments.Any(m => m.TeamManagerEmail == user.Email) || m.ScopePackages.Any(m => m.ManagerEmail == user.Email));
                return View(await interfacePoints.Where(m => m.ProjectId == project.Id).ToListAsync());
            }
            else
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
                .Include(i => i.Project)
                .Include(i => i.BOQs)
                .Include(i => i.Activities)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (interfacePoint == null)
            {
                return NotFound();
            }

            return View(interfacePoint);
        }

        // GET: InterfacePoints/Create
        [Authorize(Roles = "TeamMember")]
        public IActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var project = _context.Projects.Include(inc => inc.ScopePackages).Include(inc => inc.BOQs).Include(inc => inc.Activities).Include(i=>i.Systems)
                .Include(i=>i.Departments).FirstOrDefault(m => m.Departments.Any(m => m.TeamMembersEmails.Any(tm => tm == user.Email)));

            if (project != null )
            {
                if (project.ScopePackages != null)
                    ViewBag.ScopePackages = project.ScopePackages.Select(sp => new SelectListItem { Text = sp.Name, Value = sp.Name }).ToList();
                if (project.Systems != null)
                    ViewBag.Systems = project.Systems.Select(sp => new SelectListItem { Text = sp.Name, Value = sp.Name }).ToList();
                if (project.BOQs != null)
                    ViewBag.BOQs = project.BOQs.Select(sp => new SelectListItem { Text = sp.Name, Value = sp.Name }).ToList();
                if (project.Activities != null)
                    ViewBag.Activities = project.Activities.Select(sp => new SelectListItem { Text = sp.Name, Value = sp.Name }).ToList();

                return View();
            }
            return View();
        }

        // POST: InterfacePoints/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "TeamMember")]
        public async Task<IActionResult> Create(InterFacePointViewModel viewModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var project = _context.Projects.Include(inc => inc.ScopePackages).Include(inc => inc.BOQs).Include(inc => inc.Activities).Include(inc => inc.Departments).FirstOrDefault(m => m.Departments.Any(m => m.TeamMembersEmails.Any(tm => tm == user.Email)));


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
                Description = viewModel.Description
            };


            if (viewModel.Documentations != null)
            {
                foreach (var documentation in viewModel.Documentations)
                {
                    if (documentation.DocumentationFile != null)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", documentation.DocumentationFile.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await documentation.DocumentationFile.CopyToAsync(stream);
                        }
                        documentation.DocumentationLink = $"/uploads/{documentation.DocumentationFile.FileName}";
                    }
                    interfacePoint.Documentations.Add(documentation);
                }
                
            }
            

            foreach(var boq in viewModel.BOQs)
            {
                var BOQTable = project.BOQs.FirstOrDefault(m => m.Name == boq);
                if(BOQTable != null)
                    interfacePoint.BOQs.Add(BOQTable);
            }

            foreach(var activity in viewModel.Activities)
            {
                var activityTable = project.Activities.FirstOrDefault(m => m.Name == activity);
                if (activityTable != null)
                    interfacePoint.Activities.Add(activityTable);
            }

            _context.Add(interfacePoint);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: InterfacePoints/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interfacePoint = await _context.InterfacePoints.FindAsync(id);
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
                Description = interfacePoint.Description
            };

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var project = _context.Projects.Include(inc => inc.ScopePackages).Include(inc => inc.BOQs).Include(inc => inc.Activities).Include(i => i.Systems)
                .Include(i => i.Departments).FirstOrDefault(m => m.Departments.Any(m => m.TeamMembersEmails.Any(tm => tm == user.Email)));

            if (project != null)
            {
                if (project.ScopePackages != null)
                    ViewBag.ScopePackages = project.ScopePackages.Select(sp => new SelectListItem { Text = sp.Name, Value = sp.Name }).ToList();
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
                var project = _context.Projects.Include(inc => inc.Departments).FirstOrDefault(m => m.Departments.Any(m => m.TeamMembersEmails.Any(tm => tm == user.Email)));

                var interfacePoint = _context.InterfacePoints.FirstOrDefault(m => m.Id == id);
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


                if (viewModel.Documentations != null)
                {
                    foreach (var documentation in viewModel.Documentations)
                    {
                        if (documentation.DocumentationFile != null)
                        {
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", documentation.DocumentationFile.FileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await documentation.DocumentationFile.CopyToAsync(stream);
                            }
                            documentation.DocumentationLink = $"/uploads/{documentation.DocumentationFile.FileName}";
                        }
                        interfacePoint.Documentations.Add(documentation);
                    }

                }
                if(viewModel.BOQs != null)
                {
                    foreach (var boq in viewModel.BOQs)
                    {
                        var BOQTable = _context.BOQs.FirstOrDefault(m => m.Name == boq&&m.ProjectId==project.Id);
                        if (BOQTable != null)
                            interfacePoint.BOQs.Add(BOQTable);
                    }
                }
                
                if(viewModel.Activities != null)
                {
                    foreach (var activity in viewModel.Activities)
                    {
                        var activityTable = _context.Activities.FirstOrDefault(m => m.Name == activity && m.ProjectId == project.Id);
                        if (activityTable != null)
                            interfacePoint.Activities.Add(activityTable);
                    }
                }
                

                _context.Update(interfacePoint);
                await _context.SaveChangesAsync();
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
            var interfacePoint = await _context.InterfacePoints.FindAsync(id);
            if (interfacePoint != null)
            {
                _context.InterfacePoints.Remove(interfacePoint);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InterfacePointExists(int id)
        {
            return _context.InterfacePoints.Any(e => e.Id == id);
        }



        [HttpPost]
        public async Task<IActionResult> Issue(int id)
        {
            var interfacePoint = await _context.InterfacePoints.FindAsync(id);
            if (interfacePoint == null)
            {
                return NotFound();
            }

            interfacePoint.IssueDate = DateTime.Now;
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
