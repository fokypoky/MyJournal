namespace MyJournalLibrary.EFModels;

public class Mark
{
    public int Id { get; set; }
    public int Value { get; set; }
    public DateTime Date { get; set; }
    public int TeacherId { get; set; }
    public Employee Teacher { get; set; }
    public int TaskId { get; set; }
    public Task Task { get; set; }
}