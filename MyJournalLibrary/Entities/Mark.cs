using System;
using System.Collections.Generic;

namespace MyJournalLibrary.Entities;

public partial class Mark
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int Mark1 { get; set; }

    public DateTime MarkDate { get; set; }

    public int TeacherId { get; set; }

    public int TaskId { get; set; }

    public virtual Student Student { get; set; } = null!;

    public virtual Task Task { get; set; } = null!;

    public virtual Employee Teacher { get; set; } = null!;
}
