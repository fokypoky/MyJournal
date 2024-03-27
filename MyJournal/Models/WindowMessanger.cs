using System;

namespace MyJournal.Models;

public static class WindowMessanger
{
    public static event EventHandler MessageSender;
    public static void OnMessageSend(EventArgs e) => MessageSender?.Invoke(null, e);
}