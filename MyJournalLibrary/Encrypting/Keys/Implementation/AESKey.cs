using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJournalLibrary.Encrypting.Keys.Implementation;

public class AESKey : IEncryptionKey
{
    public byte[] Key { get; set; }
    public byte[] IV { get; set; }
    
    public AESKey(byte[] key, byte[] iv)
    {
        byte[] paddedKeyBytes = new byte[32];
        byte[] paddedIVBytes = new byte[16];

        Array.Copy(key, paddedKeyBytes, Math.Min(key.Length, paddedKeyBytes.Length));
        Array.Copy(iv, paddedIVBytes, 16);

        Key = paddedKeyBytes;
        IV = paddedIVBytes;
    }
}
