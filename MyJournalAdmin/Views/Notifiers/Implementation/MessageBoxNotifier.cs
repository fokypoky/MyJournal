using System.Windows;
using MyJournalAdmin.Views.Notifiers.Interfaces;

namespace MyJournalAdmin.Views.Notifiers.Implementation
{
    public class MessageBoxNotifier : INotifier
    {
	    public void Notify(string message)
	    {
		    MessageBox.Show(message);
	    }
    }
}
