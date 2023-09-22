using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJournalAdmin.Views.Notifiers.Interfaces
{
    public interface INotifier
    {
	    void Notify(string message);
    }
}
