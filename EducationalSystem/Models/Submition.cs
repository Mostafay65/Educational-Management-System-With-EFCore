namespace EducationalSystem.Models;

public class Submission
{
    public int Id;
    public string Solution;
    public double Grade;
    public int StudentId;
    public Student Student;
    public int AssignmentId;
    public Assignment Assignment;

    public override string ToString()
    {
        if (Grade == -1)
            return $"Your Solution : {Solution}\t Grade : NA ";
        else
            return $"Your Solution : {Solution}\t Grade : {Grade}";
    }
}