using System;
using MyJournalLibrary.Entities;

namespace MyJournalAdmin.Models.Messenging.MessageTypes
{
    public class UpdatedStudentMessage : EventArgs
    {
        public Student Student { get; set; }
    }
}
