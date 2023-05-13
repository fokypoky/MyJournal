namespace MyJournalLibrary.Entities;

public partial class    Subject
{
    public int Id { get; set; }

    public string SubjectTitle { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    public virtual ICollection<EmployeeSubject> EmployeeSubjects { get; set; }
    public virtual ICollection<Task> Tasks { get; } = new List<Task>();
    public virtual ICollection<Timetable> Timetables { get; } = new List<Timetable>();
    public virtual ICollection<ClassSubject> ClassSubjects { get; set; }
    public virtual ICollection<Class> Classes { get; set; }
    public virtual ICollection<Mark> Marks { get; set; }
}
