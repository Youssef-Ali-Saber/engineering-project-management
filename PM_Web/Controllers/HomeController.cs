using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PM.Data;
using PM.Models;
using System.Security.Claims;

namespace PM.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController( ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null) 
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login","Account");
            }
            if(User.IsInRole("Cordinator"))
            {
                return View(user);
            }
            var department = _context.Departments.FirstOrDefault(d => d.TeamManagerEmail == user.Email || d.TeamMembersEmails.Any(tm => tm == user.Email));
            if (department == null)
            {
                var scopePackages = _context.ScopePackages.FirstOrDefault(sp => sp.ManagerEmail == user.Email);
                if (scopePackages != null)
                {
                    ViewBag.ScopePackages = scopePackages;
                }
            }
            else
            {
                ViewBag.Department = department;
            }
            return View(user);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
