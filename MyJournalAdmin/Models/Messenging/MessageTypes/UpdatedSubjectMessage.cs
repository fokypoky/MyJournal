using System;
using MyJournalLibrary.Entities;

namespace MyJournalAdmin.Models.Messenging.MessageTypes
{
    public class UpdatedSubjectMessage : EventArgs
    {
        public Subject Subject { get; set; }
    }
}
