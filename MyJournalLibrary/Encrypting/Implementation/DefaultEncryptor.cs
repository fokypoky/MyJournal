namespace MyJournalLibrary.Encrypting.Implementation;

public class DefaultEncryptor : IEncryptor
{
    public string Encrypt(string text) => text;
    public string Decrypt(string text) => text;
}