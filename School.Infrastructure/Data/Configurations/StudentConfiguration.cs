using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

namespace School.Infrastructure.Data.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.Property(s => s.Name)
                   .HasColumnName("StudentName")
                   .HasColumnType("Nvarchar(100)");

            builder.Property(s => s.Email)
                   .HasColumnType("Nvarchar(60)");

            builder.HasOne(s => s.Class)
                   .WithMany()
                   .HasForeignKey(s => s.ClassId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
