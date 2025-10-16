namespace School.Domain.Entities
{
    public class Notification : BaseEntity<int>
    {
        public string UserId { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string Body { get; set; } = default!;
        public string Type { get; set; } = default!; 
        public string? SenderId { get; set; }
        public string? SenderName { get; set; }
        public string? RelatedEntityId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
        public bool WasDelivered { get; set; } = false;
        public string? FirebaseMessageId { get; set; }
    }
}
