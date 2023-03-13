namespace MyJournalLibrary.EFModels;

public class Task
{
    public int Id { get; set; }
    
    public int SubjectId { get; set; }
    public Subject Subject { get; set; }
    
    public string Text { get; set; }
    
    public int TeacherId { get; set; }
    public Employee Teacher { get; set; }
}