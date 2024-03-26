using System;
using MyJournalLibrary.Entities;

namespace MyJournalAdmin.Models.Messenging.MessageTypes
{
	public class StudentToUpdateMessage : EventArgs
	{
		public Student Student { get; set; }
	}
}