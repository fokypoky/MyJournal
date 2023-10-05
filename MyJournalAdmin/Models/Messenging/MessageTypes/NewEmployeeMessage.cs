using System;
using MyJournalLibrary.Entities;

namespace MyJournalAdmin.Models.Messenging.MessageTypes
{
    class NewEmployeeMessage : EventArgs
    {
	    public Employee Employee { get; set; }
    }
}
