using System;
using System.Collections.Generic;

namespace MyJournalLibrary.Entities;

public partial class EmployeeSubject
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public int SubjectId { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;
}
