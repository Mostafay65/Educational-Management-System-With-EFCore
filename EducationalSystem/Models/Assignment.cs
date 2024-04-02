using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationalSystem.Models;

public class Assignment
{
    public int Id;
    public string QuestionTitle;
    public int CourceId;
    public Course Course;

    public IEnumerable<Submission> Submissions { set; get; } = new List<Submission>();
}