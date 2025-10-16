namespace School.Application.Features.Chat.Dtos
{
    public class ChatMessageDto
    {
        public int Id { get; set; }
        public string SenderId { get; set; } = default!;
        public string SenderName { get; set; } = default!;
        public string SenderEmail { get; set; } = default!;
        public string? ReceiverId { get; set; } = default!;
        public string? ReceiverName { get; set; } = default!;
        public string? ReceiverEmail { get; set; } = default!;
        public string Message { get; set; } = default!;
        public DateTime SentAt { get; set; } = default!;
        public bool IsRead { get; set; } = default!;
    }
}
