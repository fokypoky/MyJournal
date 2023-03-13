namespace MyJournalLibrary.EFModels;

public class Student
{
    public int Id { get; set; }
    public int ContactsId { get; set; }
    public Contacts Contacts { get; set; }
    public int ClassId { get; set; }
    public Class Class { get; set; }
    public List<Parent> Parents { get; set; }
    public List<Mark> Marks { get; set; }
}