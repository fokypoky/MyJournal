namespace MyJournalLibrary.Encrypting;

public interface IEncryptor
{
    byte[] Encrypt(string text);
    string Decrypt(byte[] text);
}