namespace EducationalSystem.Models;

public class Student
{
    public int Id;
    public string StudentName;
    public string UserName;
    public string Password;

    public IEnumerable<Course> Courses { set; get; } = new List<Course>();
    public IEnumerable<Submission> Submissions { set; get; } = new List<Submission>();
}