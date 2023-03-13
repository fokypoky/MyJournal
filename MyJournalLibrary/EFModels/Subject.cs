namespace MyJournalLibrary.EFModels;

public class Subject
{
    public int Id { get; set; }
    public string Title { get; set; }
    public List<Employee> Employees { get; set; }
    
}