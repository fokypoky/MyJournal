using System;
using MyJournalLibrary.Entities;

namespace MyJournalAdmin.Models.Messenging.MessageTypes
{
    public class NewParentMessage : EventArgs
    {
        public Parent NewParent { get; set; }
    }
}