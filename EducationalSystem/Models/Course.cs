namespace EducationalSystem.Models;

public class Course
{
    public int Id;
    public string CourseName;
    public string Code;
    public int? DoctorId;
    public Doctor? Doctor;
    public IEnumerable<Student> Students { set; get; } = new List<Student>();
    public IEnumerable<Assignment> Assignments { set; get; } = new List<Assignment>();

    public override string ToString()
    {
        return
            $"Id = {Id}, Course Code = {Code}, Course Name = {CourseName}, Taught by = {Doctor.DoctorName}";
    }
}