using System;
using MyJournalLibrary.Entities;

namespace MyJournalAdmin.Models.Messenging.MessageTypes
{
    public class NewSubjectMessage : EventArgs
    {
        public Subject Subject { get; set; }
    }
}
