using System;
using MyJournalLibrary.Entities;

namespace MyJournalAdmin.Models.Messenging.MessageTypes
{
	public class EmployeeMessage : EventArgs
	{
		public Employee Employee { get; set; }

		public EmployeeMessage(Employee employee)
		{
			Employee = employee;
		}
	}
}
