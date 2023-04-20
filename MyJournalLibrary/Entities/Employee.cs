namespace MyJournalLibrary.Entities;

public partial class Employee
{
    public int Id { get; set; }

    public int ContactsId { get; set; }

    public virtual ICollection<Class> Classes { get; } = new List<Class>();

    public virtual Contact Contacts { get; set; } = null!;

    public virtual ICollection<Subject> Subjects { get; set; }
    public virtual ICollection<EmployeeSubject> EmployeeSubjects { get; set; } 
    public virtual ICollection<Mark> Marks { get; } = new List<Mark>();

    public virtual ICollection<Task> Tasks { get; } = new List<Task>();

    public virtual ICollection<Timetable> Timetables { get; } = new List<Timetable>();
}
