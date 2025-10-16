namespace School.Application.Features.Chat.Dtos
{
    public class ConversationSummaryDto
    {
        public string UserId { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string UserEmail { get; set; } = default!;
        public string? LastMessage { get; set; }
        public DateTime? LastMessageTime { get; set; }
        public int UnreadCount { get; set; }
    }
}
