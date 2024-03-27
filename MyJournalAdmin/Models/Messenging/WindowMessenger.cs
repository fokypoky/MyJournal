using System;

namespace MyJournalAdmin.Models.Messenging
{
	public static class WindowMessenger
	{
		public static event EventHandler MessageSender;
		public static void OnMessageSend(EventArgs e) => MessageSender?.Invoke(null, e);
	}
}
