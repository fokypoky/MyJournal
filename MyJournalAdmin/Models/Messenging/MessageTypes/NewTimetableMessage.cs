using System;
using MyJournalLibrary.Entities;

namespace MyJournalAdmin.Models.Messenging.MessageTypes
{
	public class NewTimetableMessage : EventArgs
	{
		public Timetable Timetable { get; set; }
	}
}
