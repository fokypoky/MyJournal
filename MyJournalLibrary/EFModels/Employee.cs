namespace MyJournalLibrary.EFModels;

public class Employee
{
    public int Id { get; set; }
    public int ContactsId { get; set; }
    public Contacts Contacts { get; set; }
    public List<Subject> Subjects { get; set; }
    public List<Class> Classes { get; set; }
}