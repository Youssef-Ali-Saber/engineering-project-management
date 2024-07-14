using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PM.Data;
using PM.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PM.Controllers
{
    public class ChatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Chat/Index/5
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound(); 
            }
            var interfacePoint = await _context.InterfacePoints
                .Include(ip => ip.chat)
                .FirstOrDefaultAsync(ip => ip.Id == id);

            if (interfacePoint == null)
            {
                return NotFound();
            }

            ViewBag.InterfacePointId = id;
            return View(interfacePoint.chat);
        }

        // POST: Chat/Create
        [HttpPost]
        public async Task<IActionResult> Create(int interfacePointId, string message)
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
            }

            return RedirectToAction(nameof(Index), new { id = interfacePointId });
        }
    }
}
