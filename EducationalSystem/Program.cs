// See https://aka.ms/new-console-template for more information

using EducationalSystem.Data;
using EducationalSystem.Models;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    public static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Please make a choice : ");
            Console.WriteLine(" \t 1 - Login \n \t 2 - Sign Up \n \t 3 - Shutdown system \n ");
            var choice = int.Parse(Console.ReadLine());
            if (choice == 1)
            {
                Console.Clear();
                Console.WriteLine("Please enter user name and password : ");
                Student CurrentStudent = null;
                Doctor CurrentDoctor = null;
                var Wrong = false;
                while (CurrentDoctor is null && CurrentStudent is null)
                {
                    if (Wrong)
                        Console.WriteLine("\nUsername and Password are incorrect \n \t Please enter a valid data :)\n");
                    Wrong = true;
                    Console.Write(" Username: ");
                    var Username = Console.ReadLine();
                    Console.Write(" Password: ");
                    var Password = Console.ReadLine();

                    using (var cntx = new AppDbContext())
                    {
                        CurrentStudent =
                            cntx.Students.Include(s => s.Courses)
                                .ThenInclude(c => c.Doctor)
                                .FirstOrDefault(s => s.UserName == Username && s.Password == Password);
                        CurrentDoctor =
                            cntx.Doctors.Include(d => d.Courses).ThenInclude(c => c.Students)
                                .FirstOrDefault(d => d.UserName == Username && d.Password == Password);
                    }
                }

                if (CurrentStudent is not null)
                {
                    Console.Clear();
                    Console.WriteLine("Welcome " + CurrentStudent.StudentName + ". Your are logged in :)");
                    while (true)
                    {
                        Console.WriteLine("Please make a choice :");
                        Console.WriteLine("\t 1 - Register in a Course");
                        Console.WriteLine("\t 2 - List my courses");
                        Console.WriteLine("\t 3 - View Course");
                        Console.WriteLine("\t 4 - Grades report");
                        Console.WriteLine("\t 5 - Log out");
                        choice = int.Parse(Console.ReadLine());
                        while (choice > 5 || choice < 1)
                        {
                            Console.Write("please enter a valid choice : ");
                            choice = int.Parse(Console.ReadLine());
                        }

                        if (choice == 1)
                        {
                            Console.Clear();
                            using (var cntx = new AppDbContext())
                            {
                                var Courses = cntx.Courses.Include(c => c.Doctor)
                                    .Where(c => !c.Students.Any(s => s.Id == CurrentStudent.Id));
                                if (Courses.Count() == 0)
                                {
                                    Console.WriteLine("You are registered in all our available courses :)");
                                    Console.WriteLine("\n\tpress any key to go back");
                                    Console.ReadLine();
                                    Console.Clear();
                                    continue;
                                }

                                foreach (var Course in Courses) Console.WriteLine(Course);
                                Console.Write("Which course Id to register in : ");
                                var id = int.Parse(Console.ReadLine());
                                var Register = Courses.FirstOrDefault(c => c.Id == id);
                                while (Register is null)
                                {
                                    Console.WriteLine("Please enter a valid ID");
                                    id = int.Parse(Console.ReadLine());
                                    Register = Courses.FirstOrDefault(c => c.Id == id);
                                }

                                cntx.Enrollments.Add(new Enrollment()
                                    { StudentId = CurrentStudent.Id, CourseId = Register.Id });

                                cntx.SaveChanges();
                                Console.WriteLine("You are registered in successfully\n");
                                Console.WriteLine("\n \t  To go back enter any key");
                                Console.ReadLine();
                            }
                        }
                        else if (choice == 2)
                        {
                            Console.Clear();
                            using (var cntx = new AppDbContext())
                            {
                                var courses = cntx.Courses.Where(
                                    c => c.Students.Any(s => s.Id == CurrentStudent.Id)
                                ).Include(c => c.Doctor);
                                foreach (var Course in courses) Console.WriteLine(Course);
                            }
                            Console.WriteLine("\n \t  To go back enter any key");
                            Console.ReadLine();
                        }
                        else if (choice == 3)
                        {
                            Console.Clear();
                            using (var cntx = new AppDbContext())
                            {
                                var courses = cntx.Courses.Where(
                                    c => c.Students.Any(s => s.Id == CurrentStudent.Id)
                                ).Include(c => c.Doctor);
                                foreach (var Course in courses) Console.WriteLine(Course);
                                Console.Write($"\nWhich ID course to view ? : ");
                                var id = int.Parse(Console.ReadLine());
                                var CurrentCourse = cntx.Courses
                                    .Include(c => c.Students)
                                    .Include(c => c.Doctor)
                                    .Include(c => c.Assignments)
                                    .ThenInclude(a => a.Submissions)
                                    .FirstOrDefault(c => c.Id == id);
                                if (CurrentCourse is null)
                                {
                                    Console.Write("Please enter a valid ID : ");
                                    continue;
                                }

                                Console.WriteLine(
                                    $"Id = {CurrentCourse.Id}, Course Code = {CurrentCourse.Code}, Course Name = {CurrentCourse.CourseName}");
                                Console.WriteLine($"Number of registered students = {CurrentCourse.Students.Count()}");
                                Console.WriteLine($"Taught By = {CurrentCourse.Doctor.DoctorName}");
                                foreach (var ass in CurrentCourse.Assignments)
                                {
                                    Console.WriteLine($"Asssignment : ( {ass.Id} ) {ass.QuestionTitle}");
                                    var Submissions = ass.Submissions.Where(s => s.StudentId == CurrentStudent.Id);
                                    if (Submissions.Count() == 0)
                                    {
                                        Console.WriteLine("No Submissions :) ");
                                        continue;
                                    }

                                    foreach (var Sub in Submissions) Console.WriteLine(Sub);
                                }

                                Console.WriteLine("\n \n Please make a  choice ");
                                Console.WriteLine("\t 1 - UnRegister from a Course");
                                Console.WriteLine("\t 2 - Submit solution");
                                Console.WriteLine("\t 3 - Back");
                                choice = int.Parse(Console.ReadLine());
                                while (choice < 1 || choice > 3)
                                {
                                    Console.Write("Please enter a valid code ");
                                    choice = int.Parse(Console.ReadLine());
                                }

                                if (choice == 1)
                                {
                                    var Registration = cntx.Enrollments.FirstOrDefault(e =>
                                        e.StudentId == CurrentStudent.Id && e.CourseId == CurrentCourse.Id);
                                    cntx.Enrollments.Remove(Registration);
                                    cntx.SaveChanges();
                                    Console.Clear();
                                    continue;
                                }
                                else if (choice == 2)
                                {
                                    Console.WriteLine("Which Assignment to submit?");
                                    var assid = int.Parse(Console.ReadLine());
                                    Console.WriteLine("Please Enter Your Solution ");
                                    var solution = Console.ReadLine();
                                    cntx.Submissions.Add(new Submission()
                                        { Solution = solution, StudentId = CurrentStudent.Id, AssignmentId = assid });
                                }
                                else if (choice == 3)
                                {
                                    Console.Clear();
                                    continue;
                                }

                                cntx.SaveChanges();
                            }
                        }
                        else if (choice == 4)
                        {
                            using (var cntx = new AppDbContext())
                            {
                                var subs = cntx.Submissions
                                    .Include(s => s.Assignment)
                                    .Where(s => s.StudentId == CurrentStudent.Id);
                                double sum = 0;
                                foreach (var sub in subs)
                                {
                                    Console.WriteLine(
                                        $"Assignment {sub.Assignment.Id} : {sub.Assignment.QuestionTitle}? ");
                                    Console.WriteLine($"Solution {sub.Solution}");
                                    Console.WriteLine(sub.Grade == -1 ? "NA" : $"Grage {sub.Grade} / 100");
                                    if (sub.Grade != -1) sum += sub.Grade;
                                }

                                Console.WriteLine("Total Grade is " + sum);
                            }

                            Console.WriteLine(" \n \n \t Press any key to go back");
                            Console.ReadLine();
                        }
                        else if (choice == 5)
                        {
                            Console.Write("Logging out");
                            for (var i = 0; i < 10; i++)
                            {
                                Console.Write(".");
                                Thread.Sleep(500);
                            }

                            break;
                        }

                        Console.Clear();
                    }
                }
                else if (CurrentDoctor is not null)
                {
                    Console.Clear();
                    Console.WriteLine("Welcome Dr: " + CurrentDoctor.DoctorName + ". Your are logged in :)");
                    while (true)
                    {
                        Console.WriteLine("Please make a choice :");
                        Console.WriteLine("\t 1 - List courses");
                        Console.WriteLine("\t 2 - Create course");
                        Console.WriteLine("\t 3 - View Course");
                        Console.WriteLine("\t 4 - Log out");
                        choice = int.Parse(Console.ReadLine());
                        while (choice > 4 || choice < 1)
                        {
                            Console.Write("please enter a valid choice : ");
                            choice = int.Parse(Console.ReadLine());
                        }

                        if (choice == 1)
                        {
                            Console.Clear();
                            Console.WriteLine("All availablr courses are : \n");
                            using (var cntx = new AppDbContext())
                            {
                                foreach (var c in cntx.Courses.Include(c => c.Doctor).AsNoTracking())
                                    Console.WriteLine(c);
                            }

                            Console.WriteLine("\n\n\t Press any key to go back");
                            Console.ReadLine();
                        }
                        else if (choice == 2)
                        {
                            var NewCourse = new Course();
                            Console.Write("Course name : ");
                            NewCourse.CourseName = Console.ReadLine();
                            Console.Write("Course code : ");
                            NewCourse.Code = Console.ReadLine();
                            NewCourse.DoctorId = CurrentDoctor.Id;
                            using (var cntx = new AppDbContext())
                            {
                                cntx.Courses.Add(NewCourse);
                                cntx.SaveChanges();
                            }

                            Console.WriteLine("The course is created successfully ");
                            Console.WriteLine("\n\t Press any key to go back");
                            Console.ReadLine();
                            Console.Clear();
                        }
                        else if (choice == 3)
                        {
                            Console.Clear();
                            if (CurrentDoctor.Courses.Count() == 0)
                            {
                                Console.WriteLine("You are not teaching any courses :( ");
                                Console.ReadLine();
                            }
                            else
                            {
                                foreach (var Course in CurrentDoctor.Courses) Console.WriteLine(Course);
                                Console.Write("which ID Course to view : ");
                                var id = int.Parse(Console.ReadLine());
                                while (CurrentDoctor.Courses.FirstOrDefault(c => c.Id == id) is null)
                                {
                                    Console.Write("Please enter a valid id : ");
                                    id = int.Parse(Console.ReadLine());
                                }

                                using (var cntx = new AppDbContext())
                                {
                                    var CurrentCourse = cntx.Courses
                                        .Include(c => c.Doctor)
                                        .Include(c => c.Students)
                                        .Include(c => c.Assignments)
                                        .ThenInclude(a => a.Submissions)
                                        .ThenInclude(s => s.Student)
                                        .FirstOrDefault(c => c.Id == id);
                                    Console.WriteLine(
                                        $"Id = {CurrentCourse.Id}, Course Code = {CurrentCourse.Code}, Course Name = {CurrentCourse.CourseName}");
                                    Console.WriteLine(
                                        $"Number of registered students : {CurrentCourse.Students.Count()}");
                                    Console.WriteLine($"Taught By You Dr. {CurrentCourse.Doctor.DoctorName}");
                                    foreach (var ass in CurrentCourse.Assignments)
                                    {
                                        Console.WriteLine($"Asssignment : ( {ass.Id} ) {ass.QuestionTitle}");
                                        if (ass.Submissions.Count() == 0)
                                        {
                                            Console.WriteLine("\t * No Submissions In This Assignment :) ");
                                            continue;
                                        }

                                        Console.WriteLine();
                                        foreach (var Sub in ass.Submissions)
                                        {
                                            Console.WriteLine(
                                                $"\t   Submission From Student : {Sub.Student.StudentName}");
                                            Console.WriteLine($"\t * Solution : {Sub.Solution}");
                                            Console.Write("\t * Grade : ");
                                            if (Sub.Grade == -1)
                                                Sub.Grade = double.Parse(Console.ReadLine());
                                            else Console.WriteLine(Sub.Grade);
                                            Console.WriteLine();
                                        }
                                    }

                                    Console.Write("To Add Assignment Press 1 else press 2 : ");
                                    var op = int.Parse(Console.ReadLine());
                                    if (op == 1)
                                    {
                                        Console.Write("Question Title : ");
                                        var Question = Console.ReadLine();
                                        var assignment = new Assignment() { QuestionTitle = Question };
                                        assignment.CourceId = CurrentCourse.Id;
                                        cntx.Assignments.Add(assignment);
                                    }

                                    cntx.SaveChanges();
                                }

                                Console.Write("Press any key to go back : ");
                                Console.ReadLine();
                            }
                        }
                        else if (choice == 4)
                        {
                            Console.Write("Logging out");
                            for (var i = 0; i < 10; i++)
                            {
                                Console.Write(".");
                                Thread.Sleep(500);
                            }

                            break;
                        }

                        Console.Clear();
                    }
                }
            }
            else if (choice == 2)
            {
                Console.Clear();
                Console.WriteLine("Please make a choice : You are ");
                Console.WriteLine(" \t 1 - Doctor \n \t 2 - Student \n ");
                choice = int.Parse(Console.ReadLine());
                while (choice != 1 && choice != 2)
                {
                    Console.Write("Please make a valid choice : ");
                    choice = int.Parse(Console.ReadLine());
                }

                Console.Write("Please enter your name : ");
                var Name = Console.ReadLine();
                Console.Write("Please enter your UserName : ");
                var UserName = Console.ReadLine();
                Console.Write("please enter your Password: ");
                var Password = Console.ReadLine();
                if (choice == 1)
                {
                    var User = new Doctor() { DoctorName = Name, UserName = UserName, Password = Password };
                    using (var cntx = new AppDbContext())
                    {
                        cntx.Doctors.Add(User);
                        cntx.SaveChanges();
                    }

                    Console.WriteLine("Your account is created successfully please try to login");
                    Console.ReadLine();
                }
                else
                {
                    var User = new Student() { StudentName = Name, UserName = UserName, Password = Password };
                    using (var cntx = new AppDbContext())
                    {
                        cntx.Students.Add(User);
                        cntx.SaveChanges();
                    }

                    Console.WriteLine("Your account is created successfully please try to login");
                    Console.ReadLine();
                }
            }
            else if (choice == 3)
            {
                Console.Clear();
                Console.WriteLine("Your Application is shut down");
                break;
            }
            else
            {
                Console.WriteLine("   Please enter a valid choie : ");
                Thread.Sleep(3000);
                Console.Clear();
            }
        }
    }
}