using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PM.Data;
using PM.Models;
using PM.Models.ViewModels;
using System.Security.Claims;

namespace PM.Controllers
{
    [Authorize(Roles = "Cordinator")]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Projects

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var projects = await _context.Projects.Where(m => m.OwnerId == userId).ToListAsync();
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
                .Include(p => p.Activities)
                .Include(p => p.BOQs)
                .Include(p => p.ScopePackages)
                .Include(p => p.Departments)
                .Include(p => p.Owners)
                .Include(p => p.Systems)
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
        public async Task<IActionResult> Create(ProjectViewModel viewModel)
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
                OwnerId = ownerId,
                Owners = viewModel.Owners.Select(s => new Owner { Name = s}).ToList(),
                Systems = viewModel.Systems.Select(s => new _System { Name = s }).ToList(),
                ScopePackages = viewModel.ScopePackages.Select(s => new ScopePackage { Name = s.Name, ManagerEmail = s.InterfaceManager.Email}).ToList(),
                BOQs= viewModel.BOQs.Select(b => new BOQ
                {
                    Quantity = b.Quantity,
                    Cost = b.Cost,
                    Name = b.Name,
                    Unit = b.Unit
                }).ToList(),
                Activities = viewModel.Activities.Select(a => new Activity
                {
                    Name = a.Name,
                    StartDate = a.StartDate,
                    EndDate = a.FinishDate
                }).ToList(),
                Departments = viewModel.Departments.Select(d => new Department
                {
                    Name = d.Name,
                    TeamManagerEmail = d.TeamManager.Email,
                    TeamMembersEmails = d.TeamMembers.Select(m => m.Email).ToList()
                }).ToList()
            };

            foreach (var scopePackage in viewModel.ScopePackages)
            {
                if (!await CreateUser(scopePackage.InterfaceManager,"TeamManager"))
                {
                    return View(viewModel);
                }
            }

            foreach (var department in viewModel.Departments)
            {
                foreach (var teamMember in department.TeamMembers)
                {
                    if (!await CreateUser(teamMember, "TeamMember"))
                    {
                        return View(viewModel);
                    }
                }
                if (!await CreateUser(department.TeamManager, "TeamManager"))
                {
                    return View(viewModel);
                }
            }



            
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Activities)
                .Include(p => p.BOQs)
                .Include(p => p.ScopePackages)
                .Include(p => p.Departments)
                .Include(p => p.Owners)
                .Include(p => p.Systems)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            var viewModel = new ProjectViewModel
            {
                Id = project.Id,
                ProjectName = project.ProjectName,
                Location = project.Location,
                ProjectNature = project.ProjectNature,
                ProjectType = project.ProjectType,
                JVPartners = project.JVPartners,
                ProjectValue = project.ProjectValue,
                ProjectStage = project.ProjectStage,
                DeliveryStrategies = project.DeliveryStrategies,
                ContractingStrategies = project.ContractingStrategies,
                Owners = project.Owners.Select(s => s.Name ).ToList(),
                Systems = project.Systems.Select(s => s.Name ).ToList(),
                ScopePackages = project.ScopePackages.Select(sp => new ScopePackageViewModel
                {
                    Name = sp.Name,
                    InterfaceManager = new TeamMember
                    {
                        Name = sp.ManagerEmail,  // Assuming you have a property for the name of the manager
                        Email = sp.ManagerEmail,
                        Password = "" // Assuming password is not stored in this context
                    }
                }).ToList(),
                BOQs = project.BOQs.Select(boq => new BOQViewModel
                {
                    Name = boq.Name,
                    Quantity = boq.Quantity,
                    Cost = boq.Cost,
                    Unit = boq.Unit
                }).ToList(),
                Activities = project.Activities.Select(activity => new ActivityViewModel
                {
                    Name = activity.Name,
                    StartDate = activity.StartDate,
                    FinishDate = activity.EndDate
                }).ToList(),
                Departments = project.Departments.Select(department => new DepartmentViewModel
                {
                    Name = department.Name,
                    TeamManager = new TeamMember
                    {
                        Name = department.TeamManagerEmail, // Assuming you have a property for the name of the team manager
                        Email = department.TeamManagerEmail,
                        Password = "" // Assuming password is not stored in this context
                    },
                    TeamMembers = department.TeamMembersEmails.Select(email => new TeamMember
                    {
                        Email = email,
                        Name = "", // Assuming you have a way to get the name of the team members
                        Password = "" // Assuming password is not stored in this context
                    }).ToList()
                }).ToList()
            };

            return View(viewModel);
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProjectViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Activities)
                .Include(p => p.BOQs)
                .Include(p => p.ScopePackages)
                .Include(p => p.Departments)
                .Include(p => p.Owners)
                .Include(p => p.Systems)
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
            project.ContractingStrategies = viewModel.ContractingStrategies;
            
            if (viewModel.Owners != null)
            {
                foreach (var owner in viewModel.Owners)
                {
                    project.Owners.Add(new Owner { Name = owner, projectId = project.Id });
                }
            }
            
            if (viewModel.Systems != null)
            {
                foreach (var system in viewModel.Systems)
                {
                    project.Systems.Add(new _System { Name = system, projectId = project.Id });
                }
            }

            
            // Update ScopePackages
            foreach (var spVM in viewModel.ScopePackages)
            {
                project.ScopePackages.Add(new ScopePackage
                {
                    Name = spVM.Name,
                    ManagerEmail = spVM.InterfaceManager.Email,
                    ProjectId = project.Id
                });

                if (!await CreateUser(spVM.InterfaceManager, "TeamManager"))
                {
                    return View(viewModel);
                }

            }

            // Update BOQs
            foreach (var boqVM in viewModel.BOQs)
            {
                project.BOQs.Add(new BOQ
                {
                    Name = boqVM.Name,
                    Quantity = boqVM.Quantity,
                    Cost = boqVM.Cost,
                    Unit = boqVM.Unit,
                    ProjectId = project.Id
                });
            }

            // Update Activities
            foreach (var activityVM in viewModel.Activities)
            {
                project.Activities.Add(new Activity
                {
                    Name = activityVM.Name,
                    StartDate = activityVM.StartDate,
                    EndDate = activityVM.FinishDate,
                    ProjectId = project.Id
                });
            }

            // Update Departments
            foreach (var departmentVM in viewModel.Departments)
            {
                var department = new Department
                {
                    Name = departmentVM.Name,
                    TeamManagerEmail = departmentVM.TeamManager.Email,
                    TeamMembersEmails = departmentVM.TeamMembers.Select(tm => tm.Email).ToList(),
                    ProjectId = project.Id
                };
                project.Departments.Add(department);
                foreach (var teamMember in departmentVM.TeamMembers)
                {
                    if (!await CreateUser(teamMember, "TeamMember"))
                    {
                        return View(viewModel);
                    }
                }
                if (!await CreateUser(departmentVM.TeamManager, "TeamManager"))
                {
                    return View(viewModel);
                }
            }

            try
            {
                _context.Projects.Update(project);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(viewModel.Id))
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

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
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








        //---------------------------------------------------------------------
        //---------------------------------------------------------------------
        //---------------------------------------------------------------------
        private async Task<bool> CreateUser(TeamMember teamMember,string role)
        {
            var user = await _userManager.FindByEmailAsync(teamMember.Email);
            if (user == null)
            {
                var userTeamMember = new ApplicationUser
                {
                    FullName = teamMember.Name,
                    UserName = teamMember.Email,
                    Email = teamMember.Email
                };
                var resultOfCreate = await _userManager.CreateAsync(userTeamMember, teamMember.Password);
                if (resultOfCreate.Succeeded)
                {
                    var resultRole = await _userManager.AddToRoleAsync(userTeamMember, role);
                    if (!resultRole.Succeeded)
                    {
                        var resultDelete = await _userManager.DeleteAsync(userTeamMember);
                        if (resultDelete.Succeeded)
                        {
                            ModelState.AddModelError(string.Empty, "Failed to create user");
                        }
                        else
                        {
                            foreach (var error in resultDelete.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                        foreach (var error in resultRole.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return false;
                    }
                    return true;
                }
                else
                {
                    foreach (var error in resultOfCreate.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return false;
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User already exists");
                return false;
            }


        }
    }
}
