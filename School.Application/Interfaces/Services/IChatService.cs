using School.Application.Common.Models;
using School.Application.Common.Models.QueryParams;
using School.Application.Features.Chat.Dtos;

namespace School.Application.Interfaces.Services
{
    public interface IChatService
    {
        // SendMessageAsync
        Task<ChatMessageDto> SendMessageAsync(SendMessageDto message,string senderId);

        // GetConversationAsync
        Task<PaginatedResult<ChatMessageDto>> GetConversationAsync(ChatQueryParams queryParams);

        // GetUserConversationsAsync
        Task<IEnumerable<ConversationSummaryDto>> GetUserConversationsAsync(string userId);

        // GetUnreadMessagesAsync
        Task<IEnumerable<ChatMessageDto>> GetUnreadMessagesAsync(string userId);

        // GetUnreadMessageCountAsync
        Task<int> GetUnreadMessageCountAsync(string userId);

        // MarkMessagesAsReadAsync
        Task<bool> MarkMessagesAsReadAsync(string userId, string otherUserId);

        // DeleteMessageAsync
        Task<bool> DeleteMessageAsync(int messageId, string userId);

        // EditMessageAsync
        Task<ChatMessageDto> EditMessageAsync(EditMessageDto editMessageDto, string userId);
    }
}
