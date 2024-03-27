using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJournalLibrary.Encrypting;

public interface IEncryptionKey
{
    public byte[] Key { get; set; }
    public byte[]? IV { get; set; }
}
