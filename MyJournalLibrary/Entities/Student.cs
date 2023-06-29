namespace MyJournalLibrary.Entities;

public partial class Student
{
    public int Id { get; set; }

    public int ClassId { get; set; }

    public int ContactsId { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual Contact Contacts { get; set; } = null!;
    public virtual ICollection<ParentStudent> ParentStudents { get; set; }
    public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();
    public virtual ICollection<Parent> Parents { get; set; }

}
