namespace MyJournalLibrary.Entities;

public class EmployeeSubject
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int SubjectId { get; set; }
    public Employee Employee { get; set; }
    public Subject Subject { get; set; }
}