using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.Common.Models;
using School.Application.Common.Models.QueryParams;
using School.Application.Features.Chat.Dtos;
using School.Application.Interfaces.Services;
using System.Security.Claims;

namespace School.Api.Controllers
{
    [Authorize(Roles = "Student,Teacher,Admin")]
    public class ChatController(IServiceManager _serviceManager): ApiBaseController
    {
        [HttpPost("send-message")]
        public async Task<ActionResult<ChatMessageDto>> SendMessage([FromBody] SendMessageDto message)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _serviceManager.ChatService.SendMessageAsync(message, userId);

            await _serviceManager.ChatNotificationService.NotifyMessageSentAsync(result);

            return Ok(result);
        }

        [HttpGet("conversations")]
        public async Task<ActionResult<IEnumerable<ConversationSummaryDto>>> GetUserConversations()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var conversations = await _serviceManager.ChatService.GetUserConversationsAsync(userId);
            return Ok(conversations);
        }

        [HttpGet("conversation")]
        public async Task<ActionResult<PaginatedResult<ChatMessageDto>>> GetConversation([FromQuery] ChatQueryParams queryParams)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            queryParams.SenderId = userId;
            var messages = await _serviceManager.ChatService.GetConversationAsync(queryParams);
            return Ok(messages);
        }

        [HttpGet("unread-messages")]
        public async Task<ActionResult<IEnumerable<ChatMessageDto>>> GetUnreadMessages()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var unreadMessages = await _serviceManager.ChatService.GetUnreadMessagesAsync(userId);
            return Ok(unreadMessages);
        }

        [HttpGet("unread-messages-count")]
        public async Task<ActionResult<int>> GetUnreadMessageCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var count = await _serviceManager.ChatService.GetUnreadMessageCountAsync(userId);
            return Ok(count);
        }

        [HttpPut("mark-as-read/{otherUserId}")]
        public async Task<ActionResult<bool>> MarkMessagesAsRead(string otherUserId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _serviceManager.ChatService.MarkMessagesAsReadAsync(userId, otherUserId);
            
            if(result)
            {
                await _serviceManager.ChatNotificationService.NotifyMessagesReadAsync(otherUserId, userId);
            }

            return Ok(result);
        }

        [HttpPut("edit-message")]
        public async Task<ActionResult<ChatMessageDto>> EditMessage([FromBody] EditMessageDto messageDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var editedMessage = await _serviceManager.ChatService.EditMessageAsync(messageDto, userId);
            
            await _serviceManager.ChatNotificationService.NotifyMessageEditedAsync(editedMessage);

            return Ok(editedMessage);
        }

        [HttpDelete("delete-message/{messageId}")]
        public async Task<ActionResult<bool>> DeleteMessage(int messageId, [FromQuery] string? receiverId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _serviceManager.ChatService.DeleteMessageAsync(messageId, userId);

            if (result && !string.IsNullOrEmpty(receiverId))
            {
                await _serviceManager.ChatNotificationService.NotifyMessageDeletedAsync(messageId, receiverId);
            }

            return Ok(result);
        }
    }
}
