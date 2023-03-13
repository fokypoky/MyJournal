namespace MyJournalLibrary.EFModels;

public class Timetable
{
    public int Id { get; set; }
    public int DayOfWeek { get; set; }
    public DateTime Time { get; set; }
    public int ClassId { get; set; }
    public Class Class { get; set; }
    public int SubjectId { get; set; }
    public Subject Subject { get; set; }
    public int AuditoryId { get; set; }
    public Auditory Auditory { get; set; }
    public int TeacherId { get; set; }
    public Employee Teacher { get; set; }
}