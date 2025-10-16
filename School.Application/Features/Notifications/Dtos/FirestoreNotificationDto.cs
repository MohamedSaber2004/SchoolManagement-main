namespace School.Application.Features.Notifications.Dtos
{
    public class FirestoreNotificationDto
    {
        public string NotificationId { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string Body { get; set; } = default!;
        public string Type { get; set; } = default!;
        public string? SenderId { get; set; }
        public string? SenderName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public Dictionary<string, string>? Data { get; set; }
    }
}
