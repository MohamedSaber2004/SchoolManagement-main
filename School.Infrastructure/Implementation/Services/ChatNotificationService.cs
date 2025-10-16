using Microsoft.AspNetCore.SignalR;
using School.Application.Features.Chat.Dtos;
using School.Application.Interfaces.Services;
using School.Infrastructure.Implementation.Hubs;

namespace School.Infrastructure.Implementation.Services
{
    public class ChatNotificationService(IHubContext<ChatHub> _hubContext,
                                         IFirebaseNotificationService _firebaseNotificationService) : IChatNotificationService
    {
        public async Task NotifyMessageSentAsync(ChatMessageDto message)
        {
            if (string.IsNullOrEmpty(message.ReceiverId))
                return;

            var receiverConnectionId = ChatHub.GetConnectionId(message.ReceiverId);
            if (receiverConnectionId is not null)
            {
                await _hubContext.Clients.Client(receiverConnectionId)
                    .SendAsync("Receive Private Message", message);
            }
            else
            {
                var messagePreview = message.Message.Length > 50 ? 
                                           message.Message.Substring(0, 50) + "..." :
                                           message.Message;

                await _firebaseNotificationService.SendChatNotificationAsync(
                    message.ReceiverId,   
                    message.SenderId,      
                    message.SenderName,   
                    messagePreview        
                );
            }
        }

        public async Task NotifyMessageEditedAsync(ChatMessageDto message)
        {
            if (string.IsNullOrEmpty(message.ReceiverId))
                return;

            var receiverConnectionId = ChatHub.GetConnectionId(message.ReceiverId);
            if (receiverConnectionId is not null)
            {
                await _hubContext.Clients.Client(receiverConnectionId)
                    .SendAsync("Message Edited", message);
            }
        }

        public async Task NotifyMessageDeletedAsync(int messageId, string receiverId)
        {
            if (string.IsNullOrEmpty(receiverId))
                return;

            var receiverConnectionId = ChatHub.GetConnectionId(receiverId);
            if (receiverConnectionId is not null)
            {
                await _hubContext.Clients.Client(receiverConnectionId)
                    .SendAsync("Message Deleted", messageId);
            }
        }

        public async Task NotifyMessagesReadAsync(string senderId, string receiverId)
        {
            var senderConnectionId = ChatHub.GetConnectionId(senderId);
            if (senderConnectionId is not null)
            {
                await _hubContext.Clients.Client(senderConnectionId)
                    .SendAsync("Message Read", receiverId);
            }
        }

        public async Task NotifyTypingAsync(string receiverId, string senderName)
        {
            if (string.IsNullOrEmpty(receiverId))
                return;

            var receiverConnectionId = ChatHub.GetConnectionId(receiverId);
            if (receiverConnectionId is not null)
            {
                await _hubContext.Clients.Client(receiverConnectionId)
                    .SendAsync($"{senderName} is Typing", senderName);
            }
        }
    }
}
