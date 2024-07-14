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
                return View(await interfacePoints.Include(i => i.Project).Where(m => m.Status == "Pending" && m.Status == "Approved" && m.Project.OwnerId == userId).ToListAsync());
            
            else if(User.IsInRole("TeamManager")|| User.IsInRole("TeamMember"))
            {
                var project = _context.Projects.FirstOrDefault(m => m.TeamMembers.Any(tm => tm == user.Email)||m.TeamManager ==user.Email);
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
            var project = _context.Projects.Include(inc => inc.ScopePackages).Include(i=>i.Systems).FirstOrDefault(m => m.TeamMembers.Any(tm => tm == user.Email));

            if (project != null && project.ScopePackages != null)
            {
                ViewBag.ScopePackages = new SelectList(project.ScopePackages.Select(sp => sp.Name).ToList());
                ViewBag.Systems = new SelectList(project.Systems.Select(sp => sp.Name).ToList());
                return View();
            }
            return View();
        }

        // POST: InterfacePoints/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "TeamMember")]
        public async Task<IActionResult> Create([Bind("Nature,Scope,ScopePackage1,ScopePackage2,System1,System2,ExtraSystem,Category,Responsible,Consultant,Accountable,Informed,Supported,Documentations,ProjectId")] InterFacePointViewModel viewModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var project = _context.Projects.Include(inc => inc.ScopePackages).FirstOrDefault(m => m.TeamMembers.Any(tm => tm == user.Email));

            if(viewModel.Documentations != null)
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
                }
            }
            

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
                Status = viewModel.Status,
                CreatDate = DateTime.Now,
                Documentations = viewModel.Documentations,
                ProjectId = project.Id,
            };

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

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var project = _context.Projects.Include(inc => inc.ScopePackages).Include(i => i.Systems).FirstOrDefault(m => m.TeamMembers.Any(tm => tm == user.Email));

            if (project != null && project.ScopePackages != null)
            {
                ViewBag.ScopePackages = new SelectList(project.ScopePackages.Select(sp => sp.Name).ToList());
                ViewBag.Systems = new SelectList(project.Systems.Select(sp => sp.Name).ToList());
                return View(interfacePoint);
            }

            return View(interfacePoint);
        }

        // POST: InterfacePoints/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, InterfacePoint interfacePoint)
        {
            if (id != interfacePoint.Id)
            {
                return NotFound();
            }
            try
            {
                var userId0 = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user0 = _context.Users.FirstOrDefault(u => u.Id == userId0);
                var project0 = _context.Projects.FirstOrDefault(m => m.TeamMembers.Any(tm => tm == user0.Email));

                interfacePoint.ProjectId = project0.Id;

                foreach (var documentation in interfacePoint.Documentations)
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
                }

                _context.Update(interfacePoint);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InterfacePointExists(interfacePoint.Id))
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

        // GET: InterfacePoints/Delete/5

        [Authorize(Roles = "TeamMember,TeamManager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interfacePoint = await _context.InterfacePoints
                .Include(i => i.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (interfacePoint == null)
            {
                return NotFound();
            }

            return View(interfacePoint);
        }

        // POST: InterfacePoints/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "TeamMember,TeamManager")]
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
            return RedirectToAction(nameof(Edit), new { id });
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
            return RedirectToAction(nameof(Edit), new { id });
        }
    }
}
