using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PM01.Data;
using PM01.Models;
using PM01.Models.ViewModels;
using System.Security.Claims;

namespace PM01.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Projects
        
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var projects = await _context.Projects.Where(m=>m.OwnerId==userId).ToListAsync();
            return View(projects);
        }

        // GET: Projects/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = _context.Projects
                .Include(p => p.Owners)
                .Include(p => p.ScopePackages)
                .Include(p => p.BOQs)
                .Include(p => p.Activities)
                .FirstOrDefault(p => p.Id == id);

            if (project == null)
            {
                return NotFound();
            }


            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProjectName,Location,Owners,ProjectNature,ProjectType,ScopePackages,JVPartners,ProjectValue,ProjectStage,DeliveryStrategies,ContractingStrategies,BOQs,Activities")] ProjectViewModel viewModel)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var project = new Project
            {
                ProjectName = viewModel.ProjectName,
                Location = viewModel.Location,
                ProjectNature = viewModel.ProjectNature,
                ProjectType = viewModel.ProjectType,
                JVPartners = viewModel.JVPartners,
                ProjectValue = viewModel.ProjectValue,
                ProjectStage = viewModel.ProjectStage,
                DeliveryStrategies = viewModel.DeliveryStrategies,
                ContractingStrategies = viewModel.ContractingStrategies,
                Owners = viewModel.Owners,
                ScopePackages = viewModel.ScopePackages,
                BOQs = viewModel.BOQs,
                Activities = viewModel.Activities,
                OwnerId = ownerId
            };

            _context.Add(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(inc=>inc.Owners)
                .Include(inc=>inc.Activities)
                .Include(inc=>inc.BOQs)
                .Include(inc=>inc.ScopePackages)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProjectName,Location,Owners,ProjectNature,ProjectType,ScopePackages,JVPartners,ProjectValue,ProjectStage,DeliveryStrategies,ContractingStrategies,BOQs,Activities")] ProjectViewModel viewModel, string removedItems)
        {
            
            if (id != viewModel.Id)
            {
                return NotFound();
            }
            try
            {
                var project = await _context.Projects
                .Include(inc => inc.Owners)
                .Include(inc => inc.Activities)
                .Include(inc => inc.BOQs)
                .Include(inc => inc.ScopePackages)
                .FirstOrDefaultAsync(p => p.Id == id);
                if (project == null)
                {
                    return NotFound();
                }
                project.ProjectName = viewModel.ProjectName;
                project.Location = viewModel.Location;
                project.ProjectNature = viewModel.ProjectNature;
                project.ProjectType = viewModel.ProjectType;
                project.JVPartners = viewModel.JVPartners;
                project.ProjectValue = viewModel.ProjectValue;
                project.ProjectStage = viewModel.ProjectStage;
                project.DeliveryStrategies = viewModel.DeliveryStrategies;
                project.Owners = viewModel.Owners;
                project.ScopePackages = viewModel.ScopePackages;
                project.Activities = viewModel.Activities;
                project.BOQs = viewModel.BOQs;

                _context.Projects.Update(project);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
                
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
