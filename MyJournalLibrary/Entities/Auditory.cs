namespace MyJournalLibrary.Entities;

public partial class Auditory
{
    public int Id { get; set; }

    public string AuditoryNumber { get; set; } = null!;

    public virtual ICollection<Class> Classes { get; } = new List<Class>();

    public virtual ICollection<Timetable> Timetables { get; } = new List<Timetable>();
}
