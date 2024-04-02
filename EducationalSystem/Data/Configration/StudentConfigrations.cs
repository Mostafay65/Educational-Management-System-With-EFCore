using EducationalSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalSystem.Data.Configration;

public class StudentConfigrations : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Studens");
        builder.HasKey(s => s.Id);
        builder.Property(p => p.StudentName).HasColumnType("NVARCHAR").HasMaxLength(50);
        builder.Property(p => p.UserName).HasColumnType("NVARCHAR").HasMaxLength(50);
        builder.Property(p => p.Password).HasColumnType("NVARCHAR").HasMaxLength(50);

        builder.HasMany(s => s.Courses)
            .WithMany(c => c.Students)
            .UsingEntity<Enrollment>();

        builder.HasMany(s => s.Submissions)
            .WithOne(s => s.Student)
            .HasForeignKey(s => s.StudentId).IsRequired();
    }
}