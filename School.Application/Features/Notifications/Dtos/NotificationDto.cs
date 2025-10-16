namespace School.Application.Features.Notifications.Dtos
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string Body { get; set; } = default!;
        public string Type { get; set; } = default!;
        public string? SenderId { get; set; }
        public string? SenderName { get; set; }
        public string? RelatedEntityId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public bool WasDelivered { get; set; }
    }
}
