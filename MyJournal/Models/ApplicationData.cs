using MyJournalLibrary.Encrypting;

namespace MyJournal.Models;

public static class ApplicationData
{
    public static int UserId;
    public static UserRole UserRole;
    public static IEncryptionKey AESKey;
}