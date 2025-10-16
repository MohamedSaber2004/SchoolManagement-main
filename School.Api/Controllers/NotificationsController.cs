using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.Interfaces.Services;
using System.Security.Claims;

namespace School.Api.Controllers
{
    [Authorize(Roles = "Student,Teacher,Admin")]
    public class NotificationsController(IFirebaseNotificationService _firebaseNotificationService) : ApiBaseController
    {
        [HttpGet("my-notifications")]
        public async Task<ActionResult<IEnumerable<object>>> GetMyNotifications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var notifications = await _firebaseNotificationService.GetUserNotificationsFromRealtimeDbAsync(userId);
            return Ok(notifications);
        }

        [HttpPut("{notificationId}/mark-read")]
        public async Task<ActionResult<string>> MarkNotificationAsRead(string notificationId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _firebaseNotificationService.MarkNotificationAsReadAsync(userId, notificationId);
            
            if (result)
                return Ok(new { message = "Notification marked as read" });
            
            return BadRequest(new { message = "Failed to mark notification as read" });
        }
    }
}