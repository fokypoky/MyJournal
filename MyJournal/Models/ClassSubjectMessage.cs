using System;
using MyJournalLibrary.Entities;

namespace MyJournal.Models;

public class ClassSubjectMessage : EventArgs
{
    public Class Class = null!;
    public Subject Subject = null!;
}