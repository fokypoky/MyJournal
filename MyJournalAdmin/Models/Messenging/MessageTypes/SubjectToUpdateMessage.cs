using System;
using MyJournalLibrary.Entities;

namespace MyJournalAdmin.Models.Messenging.MessageTypes
{
    public class SubjectToUpdateMessage : EventArgs
    {
	    public Subject Subject { get; set; }
    }
}
