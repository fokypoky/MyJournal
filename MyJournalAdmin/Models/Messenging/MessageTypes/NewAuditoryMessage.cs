using System;
using MyJournalLibrary.Entities;

namespace MyJournalAdmin.Models.Messenging.MessageTypes
{
	public class NewAuditoryMessage : EventArgs
	{
		public Auditory Auditory { get; set; }
	}
}
