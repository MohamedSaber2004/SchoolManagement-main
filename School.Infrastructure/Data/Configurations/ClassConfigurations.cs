using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

namespace School.Infrastructure.Data.Configurations
{
    public class ClassConfigurations : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
        {
            builder.Property(c => c.Name)
                   .HasColumnType("Nvarchar(100)")
                   .HasColumnName("ClassName");

            builder.Property(c => c.TeacherName)
                   .HasColumnType("Nvarchar(100)");
        }
    }
}
