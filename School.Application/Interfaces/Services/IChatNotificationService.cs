using School.Application.Features.Chat.Dtos;

namespace School.Application.Interfaces.Services
{
    public interface IChatNotificationService
    {
        Task NotifyMessageSentAsync(ChatMessageDto message);
        Task NotifyMessageEditedAsync(ChatMessageDto message);
        Task NotifyMessageDeletedAsync(int messageId, string receiverId);
        Task NotifyMessagesReadAsync(string senderId, string receiverId);
        Task NotifyTypingAsync(string receiverId, string senderName);
    }
}
