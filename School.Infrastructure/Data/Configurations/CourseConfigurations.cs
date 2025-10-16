using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

namespace School.Infrastructure.Data.Configurations
{
    internal class CourseConfigurations : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.Property(c => c.Name)
                   .HasColumnName("CourseName")
                   .HasColumnType("Nvarchar(100)");

            builder.Property(c => c.Description)
                   .HasColumnType("Nvarchar(255)");
        }
    }
}
