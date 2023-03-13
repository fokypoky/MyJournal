namespace MyJournalLibrary.EFModels;

public class Parent
{
    public int Id { get; set; }
    public int ContactsId { get; set; }
    public Contacts Contacts { get; set; }
    public List<Student> Students { get; set; }
}