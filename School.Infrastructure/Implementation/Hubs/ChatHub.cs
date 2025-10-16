using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using School.Application.Features.Chat.Dtos;
using School.Application.Interfaces.Services;
using System.Security.Claims;

namespace School.Infrastructure.Implementation.Hubs
{
    [Authorize(Roles = "Admin, Student, Teacher")]
    public class ChatHub(IServiceManager _serviceManager): Hub
    {
        private static readonly Dictionary<string,string> _userConnections = new();

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier)!;

            if(!string.IsNullOrEmpty(userId))
            {
                _userConnections[userId] = Context.ConnectionId;
                await Clients.Caller.SendAsync("Connected", Context.ConnectionId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier)!;

            if(!string.IsNullOrEmpty(userId))
                _userConnections.Remove(userId);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendPrivateMessage(SendMessageDto message)
        {
            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var savedMessage = await _serviceManager.ChatService.SendMessageAsync(message, userId);

            await _serviceManager.ChatNotificationService.NotifyMessageSentAsync(savedMessage);
            await Clients.Caller.SendAsync("Message Sent", savedMessage);  
        }

        public async Task NotifyTyping(string receiverId, string senderName) 
            => await _serviceManager.ChatNotificationService.NotifyTypingAsync(receiverId, senderName);

        public async Task NotifyMessageRead(string senderId, string receiverId)
        {
            await _serviceManager.ChatService.MarkMessagesAsReadAsync(receiverId, senderId);
            await _serviceManager.ChatNotificationService.NotifyMessagesReadAsync(senderId, receiverId);
        }

        public async Task NotifyMessageEdited(EditMessageDto message)
        {
            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier)!;  

            var editedMessage = await _serviceManager.ChatService.EditMessageAsync(message, userId);

            await _serviceManager.ChatNotificationService.NotifyMessageEditedAsync(editedMessage);
            await Clients.Caller.SendAsync("Message Edited", editedMessage);
        }

        public async Task NotifyMessageDeleted(int messageId, string receiverId)
        {
            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier)!;

            await _serviceManager.ChatService.DeleteMessageAsync(messageId, userId);

            await _serviceManager.ChatNotificationService.NotifyMessageDeletedAsync(messageId, receiverId);
            await Clients.Caller.SendAsync("Message Deleted", messageId);
        }

        public static string? GetConnectionId(string userId)
        {
            _userConnections.TryGetValue(userId, out var connectionId);
            return connectionId;
        }
    }
}
