namespace School.Application.Features.Chat.Dtos
{
    public class EditMessageDto
    {
        public int MessageId { get; set; }
        public string NewMessage { get; set; } = default!;
    }
}
