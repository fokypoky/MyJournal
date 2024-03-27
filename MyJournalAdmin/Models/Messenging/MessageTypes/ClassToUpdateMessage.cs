using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyJournalLibrary.Entities;

namespace MyJournalAdmin.Models.Messenging.MessageTypes
{
    public class ClassToUpdateMessage : EventArgs
    {
        public Class Class { get; set; }
    }
}
