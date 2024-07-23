using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PM.Models.ViewModels;
using PM.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PM.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace PM.Controllers
{
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
                    Email = model.Email ,
                    FullName = model.FullName,
                    Title = model.Title,
                    Organization = model.Organization,
                    PhoneNumber = model.Phone
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
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
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
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
        public IActionResult AddMember()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddMember(TeamMember teamMember)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);
                if(await CreateUser(teamMember, "ContractorTeamMember"))
                {
                    var scopePackage =  _context.ScopePackages.FirstOrDefault(f=>f.ManagerEmail == user.Email);
                    scopePackage.TeamEmails.Add(teamMember.Email);
                    _context.Update(scopePackage);
                    await _context.SaveChangesAsync();
                    return View();
                }
            } 
            return View(teamMember);
        }

        private async Task<bool> CreateUser(TeamMember teamMember, string role)
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
