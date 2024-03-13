using System;
using MyJournalLibrary.Entities;

namespace MyJournalAdmin.Models.Messenging.MessageTypes;

public class NewStudentMessage : EventArgs
{
    public Student NewStudent { get; set; }
}