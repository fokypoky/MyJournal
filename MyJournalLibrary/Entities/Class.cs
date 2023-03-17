using System;
using System.Collections.Generic;

namespace MyJournalLibrary.Entities;

public partial class Class
{
    public int Id { get; set; }

    public string ClassNumber { get; set; } = null!;

    public int LeaderId { get; set; }

    public int AuditoryId { get; set; }

    public virtual Auditory Auditory { get; set; } = null!;

    public virtual Employee Leader { get; set; } = null!;

    public virtual ICollection<Student> Students { get; } = new List<Student>();

    public virtual ICollection<Task> Tasks { get; } = new List<Task>();

    public virtual ICollection<Timetable> Timetables { get; } = new List<Timetable>();
}
