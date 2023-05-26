namespace MyJournalLibrary.Encrypting;

public interface IEncryptor
{
    string Encrypt(string text);
    string Decrypt(string text);
}