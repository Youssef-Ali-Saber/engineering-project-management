using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
            var applicationDbContext = _context.InterfacePoints.Include(i => i.Project);
            return View(await applicationDbContext.ToListAsync());
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
        public IActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var project = _context.Projects.FirstOrDefault(m => m.TeamMembers.Any(tm => tm == user.Email));

            if (project != null && project.ScopePackages != null)
            {
                ViewBag.ScopePackages = new SelectList(project.ScopePackages.Select(sp => sp.Name).ToList());
                return View();
            }
            return View();
        }


        // POST: InterfacePoints/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nature,Scope,ScopePackage1,ScopePackage2,System1,System2,ExtraSystem,Category,Responsible,Consultant,Accountable,Informed,Supported,Documentations,ProjectId")] InterFacePointViewModel ViewModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var project = _context.Projects.FirstOrDefault(m => m.TeamMembers.Any(tm => tm == user.Email));
            foreach (var documentation in ViewModel.Documentations)
                {
                    if (documentation.DocumentationFile != null)
                    {
                        // Save the file and set the DocumentationLink to the saved file path
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", documentation.DocumentationFile.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await documentation.DocumentationFile.CopyToAsync(stream);
                        }
                        documentation.DocumentationLink = $"/uploads/{documentation.DocumentationFile.FileName}";
                    }
                }
            var interfacePoint = new InterfacePoint
            {
                Nature = ViewModel.Nature,
                Scope = ViewModel.Scope,
                ScopePackage1 = ViewModel.ScopePackage1,
                ScopePackage2 = ViewModel.ScopePackage2,
                System1 = ViewModel.System1,
                System2 = ViewModel.System2,
                ExtraSystem = ViewModel.ExtraSystem,
                Category = ViewModel.Category,
                Responsible = ViewModel.Responsible,
                Consultant = ViewModel.Consultant,
                Accountable = ViewModel.Accountable,
                Informed = ViewModel.Informed,
                Supported = ViewModel.Supported,
                Status = ViewModel.Status,
                CreatDate = DateTime.Now,
                Documentations = ViewModel.Documentations,
                ProjectId = project.Id,
            };
            _context.Add(interfacePoint);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: InterfacePoints/Edit/5
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
            var project = _context.Projects.FirstOrDefault(m => m.TeamMembers.Any(tm => tm == user.Email));

            if (project != null && project.ScopePackages != null)
            {
                ViewBag.ScopePackages = new SelectList(project.ScopePackages.Select(sp => sp.Name).ToList());
            }

            return View(interfacePoint);
        }


        // POST: InterfacePoints/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: InterfacePoints/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, InterfacePoint InterfacePoint)
        {
            if (id != InterfacePoint.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userId0 = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var user0 = _context.Users.FirstOrDefault(u => u.Id == userId0);
                    var project0 = _context.Projects.FirstOrDefault(m => m.TeamMembers.Any(tm => tm == user0.Email));

                    InterfacePoint.ProjectId = project0.Id;

                    foreach (var documentation in InterfacePoint.Documentations)
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

                    _context.Update(InterfacePoint);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InterfacePointExists(InterfacePoint.Id))
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var project = _context.Projects.FirstOrDefault(m => m.TeamMembers.Any(tm => tm == user.Email));

            if (project != null && project.ScopePackages != null)
            {
                ViewBag.ScopePackages = new SelectList(project.ScopePackages.Select(sp => sp.Name).ToList());
            }
            return View(InterfacePoint);
        }


        // GET: InterfacePoints/Delete/5
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
    }
}
