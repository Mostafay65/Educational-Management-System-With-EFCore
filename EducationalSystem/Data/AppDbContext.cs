using EducationalSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EducationalSystem.Data;

public class AppDbContext : DbContext
{
    public DbSet<Assignment> Assignments { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Submission> Submissions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var Connectionstring = new ConfigurationBuilder().AddJsonFile("appsetting.json").Build()
            .GetSection("connectionstring").Value;
        optionsBuilder.UseSqlServer(Connectionstring);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}