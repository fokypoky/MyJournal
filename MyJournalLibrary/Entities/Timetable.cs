namespace MyJournalLibrary.Entities;

public partial class Timetable
{
    public int Id { get; set; }

    public int DayOfWeek { get; set; }

    public TimeOnly LessonTime { get; set; }

    public int ClassId { get; set; }

    public int SubjectId { get; set; }

    public int? AuditoryId { get; set; }

    public int? TeacherId { get; set; }

    public virtual Auditory Auditory { get; set; } = null!;

    public virtual Class Class { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;

    public virtual Employee Teacher { get; set; } = null!;
}
