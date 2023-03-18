namespace MyJournalLibrary.Entities;

public partial class Parent
{
    public int Id { get; set; }

    public int ContactsId { get; set; }

    public virtual Contact Contacts { get; set; } = null!;
    public virtual ICollection<ParentStudent> ParentStudents { get; set; }
    public virtual ICollection<Student> Students { get; set; }
}
