using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace School.Infrastructure.Data
{
    public class SchoolDbContext(DbContextOptions<SchoolDbContext> _options): DbContext(_options)
    {

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
