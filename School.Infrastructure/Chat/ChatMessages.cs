using School.Domain.Entities;
using School.Infrastructure.Identity;

namespace School.Infrastructure.Chat
{
    public class ChatMessages : BaseEntity<int>
    {
        public string Message { get; set; } = default!;
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }

        public string SenderId { get; set; } = default!;
        public string? ReceiverId { get; set; }
    }
}
