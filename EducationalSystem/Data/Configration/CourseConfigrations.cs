using EducationalSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalSystem.Data.Configration;

public class CourseConfigrations : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Courses");
        builder.HasKey(s => s.Id);
        builder.Property(p => p.CourseName).HasColumnType("NVARCHAR").HasMaxLength(50);
        builder.Property(p => p.Code).HasColumnType("NVARCHAR").HasMaxLength(50);

        builder.HasOne(c => c.Doctor)
            .WithMany(d => d.Courses)
            .HasForeignKey(c => c.DoctorId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(c => c.Assignments)
            .WithOne(a => a.Course)
            .HasForeignKey(a => a.CourceId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}