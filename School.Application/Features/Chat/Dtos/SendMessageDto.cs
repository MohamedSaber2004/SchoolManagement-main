namespace School.Application.Features.Chat.Dtos
{
    public class SendMessageDto
    {
        public string Message { get; set; } = default!;
        public string? ReceiverId { get; set; } = default!;
    }
}
