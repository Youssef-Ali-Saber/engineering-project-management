using PM.Data;
using PM.Models;

namespace PM.Services
{
    public class NotificationService
    {
        private readonly ApplicationDbContext _context;

        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void CreateNotification(string message, List<ApplicationUser> users)
        {
            var notification = new Notification
            {
                Message = message,
                CreatedAt = DateTime.Now,
                Users = users
            };

            _context.Notifications.Add(notification);
            _context.SaveChanges();
        }

        public List<Notification> GetAllNotifications(string userId)
        {
            var notifications = _context.Notifications
                .Where(n => n.Users.Any(u => u.Id == userId))
                .OrderByDescending(n => n.CreatedAt)
                .ToList();

            return notifications;
        }
    }
}
