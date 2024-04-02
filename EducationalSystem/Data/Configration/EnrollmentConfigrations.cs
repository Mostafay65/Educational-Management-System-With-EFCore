using EducationalSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalSystem.Data.Configration;

public class EnrollmentConfigrations : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.ToTable("Enrollments");
        builder.HasKey(s => new { s.CourseId, s.StudentId });
        builder.Property(p => p.StudentId).IsRequired();
        builder.Property(p => p.CourseId).IsRequired();
    }
}