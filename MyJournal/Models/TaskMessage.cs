using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = MyJournalLibrary.Entities.Task;

namespace MyJournal.Models;

public class TaskMessage : EventArgs
{
    public Task Task = null!;
    public TaskMessageType Type;
}
