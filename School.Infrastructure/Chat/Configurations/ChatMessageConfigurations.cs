using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace School.Infrastructure.Chat.Configurations
{
    public class ChatMessageConfigurations : IEntityTypeConfiguration<ChatMessages>
    {
        public void Configure(EntityTypeBuilder<ChatMessages> builder)
        {
            builder.Property(ch => ch.Message)
                   .IsRequired()
                   .HasMaxLength(1500);

            builder.Property(ch => ch.SentAt)
                   .IsRequired()
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(ch => ch.IsRead)
                   .HasDefaultValue(false);

            builder.HasIndex(ch => new {ch.SenderId,ch.ReceiverId, ch.SentAt})
                   .HasDatabaseName("IX_ChatMessages_Conversation");
        }
    }
}
