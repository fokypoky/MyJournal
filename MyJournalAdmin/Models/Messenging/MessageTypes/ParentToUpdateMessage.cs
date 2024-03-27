using System;
using MyJournalLibrary.Entities;

namespace MyJournalAdmin.Models.Messenging.MessageTypes
{
	public class ParentToUpdateMessage : EventArgs
	{
		public Parent Parent { get; set; }
	}
}
