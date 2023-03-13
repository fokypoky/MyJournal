namespace MyJournalLibrary.EFModels;

public class Class
{
    public int Id { get; set; }
    public string Number { get; set; }
    
    public int LeaderId { get; set; }
    public Employee Leader { get; set; }
    
    public int? AuditoryId { get; set; }
    public Auditory? Auditory { get; set; }
    
    public List<Timetable> Timetable { get; set; }
    public List<Student> Students { get; set; }
}