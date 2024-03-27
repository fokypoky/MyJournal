using System;
using MyJournalLibrary.Entities;

namespace MyJournalAdmin.Models.Messenging.MessageTypes
{
    public class NewClassMessage : EventArgs
    {
        public Class Class { get; set; }
    }
}
