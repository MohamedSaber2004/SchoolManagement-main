using School.Application.Common.Models.QueryParams;
using School.Infrastructure.Chat;

namespace School.Infrastructure.Implementation.Specifications.ChatModuleSpecifications
{
    public class ChatSpecifications: BaseSpecification<ChatMessages,int>
    {
        public ChatSpecifications(ChatQueryParams queryParams)
            :base(ch =>
                     (ch.SenderId == queryParams.SenderId && ch.ReceiverId == queryParams.ReceiverId)
                  || (ch.SenderId == queryParams.ReceiverId && ch.ReceiverId == queryParams.SenderId))
        {
            AddOrderByDesc(ch => ch.SentAt);

            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
        }

        public ChatSpecifications(string userId)
            : base(ch => ch.ReceiverId == userId && !ch.IsRead)
        {
            AddOrderByDesc(ch => ch.SentAt);
        }

        public ChatSpecifications(string userId, string otherUserId)
            : base(m => (m.SenderId == otherUserId && m.ReceiverId == userId && !m.IsRead))
        {
            
        }
    }
}
