using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PM.Data;
using PM.Models;
using PM.Models.ViewModels;
using System.Security.Claims;

namespace PM.Controllers;

public class AccountController : Controller
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ApplicationDbContext _context;
    public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
        SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _context = context;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Password and Confirm Password do not match");
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                ModelState.AddModelError(string.Empty, "User with this email already exists");
                return View(model);
            }
            user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                Title = model.Title,
                Organization = model.Organization,
                PhoneNumber = model.Phone
            };
            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded)
            {
                user.PasswordHash = model.Password;

                await _userManager.UpdateAsync(user);

                if (!await _roleManager.RoleExistsAsync("Cordinator"))
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole("Cordinator"));
                    if (!roleResult.Succeeded)
                    {
                        foreach (var error in roleResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        await _userManager.DeleteAsync(user);
                        return View(model);
                    }
                }
                var resultRole = await _userManager.AddToRoleAsync(user, "Cordinator");
                if (resultRole.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var resultDelete = await _userManager.DeleteAsync(user);
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
                }
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }


    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && user.PasswordHash == model.Password)
            {
                await _signInManager.SignInAsync(user, model.RememberMe);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> AddMember()
    {

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);

        var projects = await _context.Projects
            .Include(s => s.ScopePackages)
            .Where(f => f.ScopePackages.Any(x => x.ManagerEmail == user.Email)).ToListAsync();

        ViewBag.projects = projects.Select(c => new SelectListItem(c.ProjectName, c.Id.ToString()));

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddMember(TeamMember teamMember, [FromForm] int projectId)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);
        if (!await CreateUser(teamMember, "ContractorTeamMember"))
        {
            return View();
        }
        var scopePackages = _context.ScopePackages.Where(f => f.ManagerEmail == user.Email && f.ProjectId == projectId);

        foreach (var scopePackage in scopePackages)
        {
            scopePackage.TeamEmails.Add(teamMember.Email);
            _context.ScopePackages.Update(scopePackage);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Users(int? projectId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);

        IEnumerable<Project> projects = [];

        IEnumerable<ApplicationUser> users = [];

        if (User.IsInRole("Cordinator"))
        {


            projects = _context.Projects
                .Include(x => x.ScopePackages)
                .Include(x => x.Departments)
                .Where(f => f.OwnerId == user.Id);


            users = _context.Users
                .Where(f => projects.Any(c => c.Departments.Any(v => v.TeamManagerEmail == f.Email))
                            || projects.Any(c => c.Departments.Any(v => v.TeamMembersEmails.Any(b => b == f.Email)))
                            || projects.Any(x => x.ScopePackages.Any(n => n.ManagerEmail == f.Email)));

        }
        else if (User.IsInRole("Contractor"))
        {
            projects = _context.Projects
                .Include(x => x.ScopePackages)
                .Where(f => f.ScopePackages.Any(c => c.ManagerEmail == user.Email));

            users = _context.Users
                .Where(f => projects.Any(x => x.ScopePackages.Any(n => n.ManagerEmail == user.Email && n.TeamEmails.Any(z => z == f.Email))));

        }
        else if (User.IsInRole("TeamManager"))
        {
            projects = _context.Projects
                .Include(x => x.Departments)
                .Where(f => f.Departments.Any(c => c.TeamManagerEmail == user.Email));

            users = _context.Users
                .Where(f => projects.Any(c => c.Departments.Any(v => v.TeamManagerEmail == user.Email && v.TeamMembersEmails.Any(b => b == f.Email))));

        }
        else
        {
            return NotFound("Not have access");
        }

        var usersList = users.Select(c => new User
        {
            Name = c.FullName,
            Email = c.Email,
            Password = c.PasswordHash,
            Role = _userManager.GetRolesAsync(c).Result.FirstOrDefault()
        }).ToList();

        ViewBag.projects = projects.Select(c => new SelectListItem(c.ProjectName, c.Id.ToString())).ToList();

        return View(usersList);
    }


    private async Task<bool> CreateUser(TeamMember teamMember, string role)
    {
        var user = await _userManager.FindByEmailAsync(teamMember.Email);
        if (user != null)
        {
            return true;
        }
        var userTeamMember = new ApplicationUser
        {
            FullName = teamMember.Name,
            UserName = teamMember.Email,
            Email = teamMember.Email
        };
        var resultOfCreate = await _userManager.CreateAsync(userTeamMember);
        if (resultOfCreate.Succeeded)
        {
            userTeamMember.PasswordHash = teamMember.Password;

            await _userManager.UpdateAsync(userTeamMember);

            var resultRole = await _userManager.AddToRoleAsync(userTeamMember, role);
            if (resultRole.Succeeded)
            {
                return true;

            }
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

        foreach (var error in resultOfCreate.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
        return false;

    }

}
