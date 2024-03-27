using System;
using MyJournalLibrary.Entities;

namespace MyJournalAdmin.Models.Messenging.MessageTypes
{
    public class AuditoryEditMessage : EventArgs
    {
	    public Auditory Auditory { get; set; } = null!;
    }
}
