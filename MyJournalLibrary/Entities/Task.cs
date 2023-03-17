using System;
using System.Collections.Generic;

namespace MyJournalLibrary.Entities;

public partial class Task
{
    public int Id { get; set; }

    public int SubjectId { get; set; }

    public int ClassId { get; set; }

    public string TaskText { get; set; } = null!;

    public int TeacherId { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual ICollection<Mark> Marks { get; } = new List<Mark>();

    public virtual Subject Subject { get; set; } = null!;

    public virtual Employee Teacher { get; set; } = null!;
}
