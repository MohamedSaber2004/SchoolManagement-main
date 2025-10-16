namespace School.Application.Interfaces.Services
{
    public interface IFirebaseNotificationService
    {
        Task<string?> SendNotificationAsync(string deviceToken, string title, string body, Dictionary<string, string>? data = null);

        Task SendChatNotificationAsync(string receiverUserId, string senderUserId, string senderName, string message);

        Task<bool> StoreNotificationInRealtimeDbAsync(string userId, string title, string body, string type, Dictionary<string, string>? data = null);

        Task<IEnumerable<object>> GetUserNotificationsFromRealtimeDbAsync(string userId);

        Task<bool> MarkNotificationAsReadAsync(string userId, string notificationId);
    }
}