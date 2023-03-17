using System;
using System.Collections.Generic;

namespace MyJournalLibrary.Entities;

public partial class Subject
{
    public int Id { get; set; }

    public string SubjectTitle { get; set; } = null!;

    public virtual ICollection<EmployeeSubject> EmployeeSubjects { get; } = new List<EmployeeSubject>();

    public virtual ICollection<Task> Tasks { get; } = new List<Task>();

    public virtual ICollection<Timetable> Timetables { get; } = new List<Timetable>();
}
