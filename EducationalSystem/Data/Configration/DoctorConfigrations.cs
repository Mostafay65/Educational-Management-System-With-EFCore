using EducationalSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalSystem.Data.Configration;

public class DoctorConfigrations : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.ToTable("Doctors");
        builder.HasKey(s => s.Id);
        builder.Property(p => p.DoctorName).HasColumnType("NVARCHAR").HasMaxLength(50);
        builder.Property(p => p.UserName).HasColumnType("NVARCHAR").HasMaxLength(50);
        builder.Property(p => p.Password).HasColumnType("NVARCHAR").HasMaxLength(50);
    }
}