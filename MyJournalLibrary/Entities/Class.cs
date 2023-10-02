namespace MyJournalLibrary.Entities;

public partial class Class
{
    public int Id { get; set; }

    public string ClassNumber { get; set; } = null!;

    public int? LeaderId { get; set; }

    public int AuditoryId { get; set; }

    public virtual Auditory Auditory { get; set; } = null!;

    public virtual Employee? Leader { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    public virtual ICollection<Timetable> Timetables { get; set; } = new List<Timetable>();
    public virtual ICollection<ClassSubject> ClassSubjects { get; set; } = new List<ClassSubject>();
    public virtual ICollection<Subject> Subjects { get; set; }

}
