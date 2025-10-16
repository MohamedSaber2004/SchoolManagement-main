using School.Application.Common.Models.QueryParams;
using School.Infrastructure.Chat;

namespace School.Infrastructure.Implementation.Specifications.ChatModuleSpecifications
{
    public class ChatCountSpecifications : BaseSpecification<ChatMessages, int>
    {
        public ChatCountSpecifications(ChatQueryParams queryParams)
             : base(ch =>
                     (ch.SenderId == queryParams.SenderId && ch.ReceiverId == queryParams.ReceiverId)
                  || (ch.SenderId == queryParams.ReceiverId && ch.ReceiverId == queryParams.SenderId))
        {

        }

        public ChatCountSpecifications(string userId)
            : base(ch => ch.ReceiverId == userId && !ch.IsRead)
        {
        }
    }
}
