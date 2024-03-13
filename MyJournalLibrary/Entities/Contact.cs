namespace MyJournalLibrary.Entities;

public partial class Contact
{
    public int Id { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Midname { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; }

    public string Password { get; set; } = null!;

    public string Sex { get; set; } = null!;

    public int? UserRoleId { get; set; }
    public virtual UserRole UserRole { get; set; }
    public virtual ICollection<Employee> Employees { get; } = new List<Employee>();

    public virtual ICollection<Parent> Parents { get; } = new List<Parent>();

    public virtual ICollection<Student> Students { get; } = new List<Student>();
}
