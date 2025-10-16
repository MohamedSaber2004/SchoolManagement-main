using Microsoft.EntityFrameworkCore;
using School.Infrastructure.Chat.Configurations;
using System.Reflection;

namespace School.Infrastructure.Data
{
    public class ChatDbContext(DbContextOptions<ChatDbContext> _options): DbContext(_options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ChatMessageConfigurations());
        }
    }
}
