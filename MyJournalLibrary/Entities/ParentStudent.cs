using System;
using System.Collections.Generic;

namespace MyJournalLibrary.Entities;

public partial class ParentStudent
{
    public int Id { get; set; }

    public int ParentId { get; set; }

    public int StudentId { get; set; }

    public virtual Parent Parent { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
