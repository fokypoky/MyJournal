namespace MyJournalLibrary.Entities;

public partial class UserRole
{
    public int Id { get; set; }

    public string Rolename { get; set; } = null!;
    public virtual ICollection<Contact> Contacts { get; set; }
}
