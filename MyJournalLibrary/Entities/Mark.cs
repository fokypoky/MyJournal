namespace MyJournalLibrary.Entities;

public partial class Mark
{
    public int Id { get; set; }

    public int StudentId { get; set; }
    public int SubjectId { get; set; }

    public int MarkValue { get; set; }

    public DateTime MarkDate { get; set; }

    public int TeacherId { get; set; }

    public int? TaskId { get; set; }

    public virtual Student Student { get; set; } = null!;

    public virtual Task Task { get; set; } = null!;

    public virtual Employee Teacher { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;
}
