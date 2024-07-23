using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PM.Data;
using PM.Models;
using PM.Services;

namespace PM.Controllers
{
    public class NotificationsController : Controller
    {
            private readonly NotificationService _notificationService;

        public NotificationsController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }


        public IActionResult GetAllNotifications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notifications = _notificationService.GetAllNotifications(userId);
            return View(notifications);
        }


    }
}
