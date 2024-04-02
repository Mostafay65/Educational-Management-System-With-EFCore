namespace EducationalSystem.Models;

public class Doctor
{
    public int Id;
    public string DoctorName;
    public string UserName;
    public string Password;
    public IEnumerable<Course> Courses { set; get; } = new List<Course>();
}