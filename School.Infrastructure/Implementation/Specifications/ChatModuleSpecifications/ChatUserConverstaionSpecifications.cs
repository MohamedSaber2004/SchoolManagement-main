using School.Infrastructure.Chat;

namespace School.Infrastructure.Implementation.Specifications.ChatModuleSpecifications
{
    public class ChatUserConverstaionSpecifications: BaseSpecification<ChatMessages,int>
    {
        public ChatUserConverstaionSpecifications(string userId)
            :base(m => m.SenderId == userId || m.ReceiverId == userId)
        {
            
        }
    }
}
