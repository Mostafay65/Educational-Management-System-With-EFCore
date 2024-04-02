using EducationalSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalSystem.Data.Configration;

public class SubmissionConfigrations : IEntityTypeConfiguration<Submission>
{
    public void Configure(EntityTypeBuilder<Submission> builder)
    {
        builder.ToTable("Submissions");
        builder.HasKey(s => s.Id);
        builder.Property(p => p.Solution).HasColumnType("NVARCHAR").HasMaxLength(1000);
        builder.Property(p => p.Grade).HasColumnName("Grade").HasDefaultValue(-1);
    }
}