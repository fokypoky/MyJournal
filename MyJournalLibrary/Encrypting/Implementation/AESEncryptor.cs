using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MyJournalLibrary.Encrypting.Keys.Implementation;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Numerics;

namespace MyJournalLibrary.Encrypting.Implementation;

public class AESEncryptor : IEncryptor
{
    private IEncryptionKey _encryptionKey;

    public AESEncryptor(IEncryptionKey encryptionKey)
    {
        _encryptionKey = (AESKey) encryptionKey;
    }
    public byte[] Encrypt(string cipherText)
    {
        byte[] encrypted;

        using (Aes aes = Aes.Create())
        {
            aes.Key = _encryptionKey.Key;
            aes.IV = _encryptionKey.IV;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(cipherText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }

            return encrypted;
        }

        return encrypted;
    }

    public string Decrypt(byte[] cipherText)
    {
        string decryptedText = null;

        using (Aes aes = Aes.Create())
        {
            aes.Key = _encryptionKey.Key;
            aes.IV = _encryptionKey.IV;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        decryptedText = srDecrypt.ReadToEnd();
                    }
                }
            }
        }

        return decryptedText;
    }
}
