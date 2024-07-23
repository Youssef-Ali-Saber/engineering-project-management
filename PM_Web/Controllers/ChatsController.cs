using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PM.Data;
using PM.Models;
using PM.Services;

namespace PM.Controllers
{
    public class ChatsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly NotificationService _notificationService;

        public ChatsController(ApplicationDbContext context, NotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        // GET: Chat/Index/5
        public async Task<IActionResult> ChatsOfInterfacePoint(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var interfacePoint = await _context.InterfacePoints
                .Include(ip => ip.Chat)
                .FirstOrDefaultAsync(ip => ip.Id == id);

            if (interfacePoint == null)
            {
                return NotFound();
            }

            ViewBag.InterfacePointId = id;
            return View(interfacePoint.Chat);
        }

        // POST: Chat/Create
        [HttpPost]
        public async Task<IActionResult> AddMessageToChatOfInterfacePoint(int interfacePointId, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                var chat = new Chat
                {
                    InterfacePointId = interfacePointId,
                    Message = message,
                    Sender = User.Identity.Name,
                    Time = DateTime.Now
                };

                _context.Chats.Add(chat);
                await _context.SaveChangesAsync();

                var allUsers = _context.Users.ToList();

                var project = _context.InterfacePoints.Include(m => m.Project).ThenInclude(m => m.Departments).FirstOrDefault(n => n.Id == interfacePointId)?.Project;
                var user = allUsers.FirstOrDefault(n => n.Email == User.Identity.Name);

                var usersToNotify = allUsers.Where(m =>
                    m.Id == project.OwnerId ||
                    project.Departments.Any(n => n.TeamMembersEmails.Contains(m.Email) || n.TeamManagerEmail == m.Email)).ToList();

                if (usersToNotify.Contains(user))
                {
                    usersToNotify.Remove(user);
                }
                if (User.IsInRole("TeamMember")|| User.IsInRole("TeamManager"))
                    _notificationService.CreateNotification($"{user.FullName} From Department {_context.Departments.FirstOrDefault(n => n.TeamMembersEmails.Contains(user.Email)||n.TeamManagerEmail==user.Email).Name} sended new massage on Interface Point IP_{interfacePointId} chat", usersToNotify);
                if (User.IsInRole("Cordinator"))
                    _notificationService.CreateNotification($"Cordinator sended new massage on Interface Point IP_{interfacePointId} chat", usersToNotify);

            }

            return RedirectToAction(nameof(ChatsOfInterfacePoint), new { id = interfacePointId });
        }

        public async Task<IActionResult> ChatsOfInterfaceAgreement(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var InterfaceAgreement = await _context.InterfaceAgreements
                .Include(ip => ip.Chat)
                .FirstOrDefaultAsync(ip => ip.Id == id);

            if (InterfaceAgreement == null)
            {
                return NotFound();
            }

            ViewBag.InterfaceAgreementId = id;
            return View(InterfaceAgreement.Chat);
        }

        // POST: Chat/Create
        [HttpPost]
        public async Task<IActionResult> AddMessageToChatOfInterfaceAgreement(int interfaceAgreementId, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                var chat = new Chat
                {
                    InterfaceAgreementId = interfaceAgreementId,
                    Message = message,
                    Sender = User.Identity.Name,
                    Time = DateTime.Now
                };

                _context.Chats.Add(chat);
                await _context.SaveChangesAsync();

                var allUsers = _context.Users.ToList();

                var interfacePoint = _context.InterfaceAgreements.Include(m => m.InterfacePoint).ThenInclude(m => m.Project).ThenInclude(m => m.ScopePackages).FirstOrDefault(n => n.Id == interfaceAgreementId)?.InterfacePoint;
                var user = allUsers.FirstOrDefault(n => n.Email == User.Identity.Name);

                var usersToNotify = allUsers.Where(m =>
                    m.Id == interfacePoint.Project.OwnerId ||
                    m.Email == interfacePoint.Supported ||
                    m.Email == interfacePoint.Accountable ||
                    m.Email == interfacePoint.Consultant ||
                    m.Email == interfacePoint.Responsible ||
                    m.Email == interfacePoint.Informed 
                    ).ToList();

                foreach(var scopePackage in interfacePoint.Project.ScopePackages)
                {
                    foreach(var email in scopePackage.TeamEmails)
                    {
                        usersToNotify.Add(allUsers.FirstOrDefault(n => n.Email == email));
                    }
                }

                if (usersToNotify.Contains(user))
                {
                    usersToNotify.Remove(user);
                }
                if (User.IsInRole("ContractorTeamMember") || User.IsInRole("Contractor"))
                    _notificationService.CreateNotification($"{user.FullName} From ScopePackage {_context.ScopePackages.FirstOrDefault(n => n.TeamEmails.Contains(user.Email)||n.ManagerEmail ==user.Email).Name} sended new massage on Interface Agreement IA_{interfaceAgreementId} chat", usersToNotify);
                if (User.IsInRole("Cordinator"))
                    _notificationService.CreateNotification($"Cordinator sended new massage on Interface Agreement IA_{interfaceAgreementId} chat", usersToNotify);

            }

            return RedirectToAction(nameof(ChatsOfInterfaceAgreement), new { id = interfaceAgreementId });
        }
    }

}
