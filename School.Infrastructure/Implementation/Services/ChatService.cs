using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using School.Application.Common.Exceptions;
using School.Application.Common.Models;
using School.Application.Common.Models.QueryParams;
using School.Application.Features.Chat.Dtos;
using School.Application.Interfaces.Repositories;
using School.Application.Interfaces.Services;
using School.Infrastructure.Chat;
using School.Infrastructure.Identity;
using School.Infrastructure.Implementation.Specifications.ChatModuleSpecifications;

namespace School.Infrastructure.Implementation.Services
{
    public class ChatService(IChatUnitOfWork _unitOfWork,
                             UserManager<ApplicationUser> _userManager) : IChatService
    {
        public async Task<bool> DeleteMessageAsync(int messageId, string userId)
        {
            var message = await _unitOfWork.GetRepository<ChatMessages,int>().GetByIdAsync(messageId)
                                    ?? throw new MessageNotFoundException(messageId);

            if (message.SenderId != userId)
                throw new UnauthorizedException();

            _unitOfWork.GetRepository<ChatMessages, int>().Delete(message);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<ChatMessageDto> EditMessageAsync(EditMessageDto editMessageDto, string userId)
        {
            var message = await _unitOfWork.GetRepository<ChatMessages,int>().GetByIdAsync(editMessageDto.MessageId)
                                    ?? throw new MessageNotFoundException(editMessageDto.MessageId);
            
            if(message.SenderId != userId)
                throw new UnauthorizedException();

            message.Message = editMessageDto.NewMessage;
            _unitOfWork.GetRepository<ChatMessages, int>().Update(message);
            await _unitOfWork.SaveChangesAsync();

            var sender = await _userManager.FindByIdAsync(message.SenderId)
                            ?? throw new UserNotFoundException(message.SenderId);
            var receiver = message.ReceiverId != null ? await _userManager.FindByIdAsync(message.ReceiverId) : null;

            return new ChatMessageDto
            {
                Id = message.Id,
                SenderId = sender.Id,
                SenderName = sender.DisplayName,
                SenderEmail = sender.Email ?? string.Empty,
                ReceiverId = receiver?.Id,
                ReceiverName = receiver?.DisplayName,
                ReceiverEmail = receiver?.Email ?? string.Empty,
                SentAt = DateTime.Now,
                Message = message.Message,
                IsRead = message.IsRead
            };
        }

        public async Task<PaginatedResult<ChatMessageDto>> GetConversationAsync(ChatQueryParams queryParams)
        {
            var spec = new ChatSpecifications(queryParams);
            var messages = await _unitOfWork.GetRepository<ChatMessages, int>().GetAllAsync(spec);
            var totalMessages = messages.Count();

            var sender = await _userManager.FindByIdAsync(queryParams.SenderId)
                            ?? throw new UserNotFoundException(queryParams.SenderId);

            var receiver = queryParams.ReceiverId != null ? await _userManager.FindByIdAsync(queryParams.ReceiverId) : null;
            var messageDtos = messages.Select(message => new ChatMessageDto()
            {
                Id = message.Id,
                SenderId = message.SenderId,
                SenderName = sender.DisplayName,
                SenderEmail = sender.Email ?? string.Empty,
                ReceiverId = message.ReceiverId,
                ReceiverName = receiver?.DisplayName,
                ReceiverEmail = receiver?.Email ?? string.Empty,
                Message = message.Message,
                SentAt = message.SentAt,
                IsRead = message.IsRead
            });
            var countSpec = new ChatCountSpecifications(queryParams);
            var totalCount = await _unitOfWork.GetRepository<ChatMessages,int>().CountAsync(countSpec);
            return new PaginatedResult<ChatMessageDto>(queryParams.PageIndex, messageDtos.Count(), totalCount, messageDtos);
        }

        public async Task<int> GetUnreadMessageCountAsync(string userId)
            => await _unitOfWork.GetRepository<ChatMessages,int>().CountAsync(new ChatSpecifications(userId));

        public async Task<IEnumerable<ChatMessageDto>> GetUnreadMessagesAsync(string userId)
        {
            var unreadMessages = await _unitOfWork.GetRepository<ChatMessages, int>().GetAllAsync(new ChatSpecifications(userId));
            
            var senderIds = unreadMessages.Select(m => m.SenderId).Distinct().ToList();

            var users = await _userManager.Users.Where(u => senderIds.Contains(u.Id))
                        .ToDictionaryAsync(u => u.Id, u => u);

            var currentUser = await _userManager.FindByIdAsync(userId)
                                ?? throw new UserNotFoundException(userId);

            return unreadMessages.Select(message =>
            {
                var sender = users.GetValueOrDefault(message.SenderId);
                return new ChatMessageDto()
                {
                    Id = message.Id,
                    SenderId = message.SenderId,
                    SenderName = sender?.DisplayName ?? "Unknown",
                    SenderEmail = sender?.Email ?? string.Empty,
                    ReceiverId = message.ReceiverId,
                    ReceiverName = currentUser.DisplayName,
                    ReceiverEmail = currentUser.Email ?? string.Empty,
                    Message = message.Message,
                    SentAt = message.SentAt,
                    IsRead = message.IsRead
                };
            });
        }

        public async Task<IEnumerable<ConversationSummaryDto>> GetUserConversationsAsync(string userId)
        {
            var currentUser = await _userManager.FindByIdAsync(userId)
                                ?? throw new UserNotFoundException(userId);

            var userMessages = await _unitOfWork.GetRepository<ChatMessages, int>()
                                .GetAllAsync(new ChatUserConverstaionSpecifications(userId));

            if (!userMessages.Any())
                return Enumerable.Empty<ConversationSummaryDto>();

            var conversations = userMessages
                                  .GroupBy(m => m.SenderId == userId ? m.ReceiverId : m.SenderId)
                                  .Where(g => g.Key != null)
                                  .Select(g => new
                                  {
                                      OtherUserId = g.Key!,
                                      Messages = g.OrderByDescending(m => m.SentAt).ToList()
                                  }).ToList();

            var otherUserIds = conversations.Select(c => c.OtherUserId).Distinct().ToList();

            var users = await _userManager.Users
                        .Where(u => otherUserIds.Contains(u.Id))
                        .ToDictionaryAsync(u => u.Id, u => u);

            var conversationSummaries = conversations.Select(conversation =>
            {
                var lastMessage = conversation.Messages.First();
                var unreadCount = conversation.Messages.Count(m => m.ReceiverId == userId && !m.IsRead);

                var otherUser = users.GetValueOrDefault(conversation.OtherUserId);

                return new ConversationSummaryDto()
                {
                    UserId = conversation.OtherUserId,
                    UserName = otherUser?.DisplayName ?? "Unknown",
                    UserEmail = otherUser?.Email ?? string.Empty,
                    LastMessage = lastMessage.Message,
                    LastMessageTime = lastMessage.SentAt,
                    UnreadCount = unreadCount
                };
            }).OrderByDescending(c => c.LastMessageTime).ToList();

            return conversationSummaries;
        }

        public async Task<bool> MarkMessagesAsReadAsync(string userId, string otherUserId)
        {
            var messages = await _unitOfWork.GetRepository<ChatMessages,int>()
                            .GetAllAsync(new ChatSpecifications(userId, otherUserId));

            if(!messages.Any())
                return false;

            foreach (var message in messages)
                message.IsRead = true;

            _unitOfWork.GetRepository<ChatMessages,int>().UpdateRange(messages);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<ChatMessageDto> SendMessageAsync(SendMessageDto message, string senderId)
        {
            var sender = await _userManager.FindByIdAsync(senderId)
                            ?? throw new UserNotFoundException(senderId);

            var receiver  = message.ReceiverId != null ? await _userManager.FindByIdAsync(message.ReceiverId) : null;

            var chatMessage = new ChatMessages()
            {
                Message = message.Message,
                SenderId = senderId,
                ReceiverId = message.ReceiverId,
                SentAt = DateTime.Now,
                IsRead = false
            };

            await _unitOfWork.GetRepository<ChatMessages,int>().AddAsync(chatMessage);
            await _unitOfWork.SaveChangesAsync();

            return new ChatMessageDto()
            {
                Id = chatMessage.Id,
                SenderId = sender.Id,
                SenderName = sender.DisplayName,
                SenderEmail = sender.Email ?? string.Empty,
                ReceiverId = receiver?.Id,
                ReceiverName = receiver?.DisplayName,
                ReceiverEmail = receiver?.Email ?? string.Empty,
                Message = chatMessage.Message,
                SentAt = chatMessage.SentAt,
                IsRead = chatMessage.IsRead
            };
        }
    }
}
