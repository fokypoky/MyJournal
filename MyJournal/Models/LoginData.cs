using System.Runtime.Serialization;
using MyJournalLibrary.Encrypting;
using MyJournalLibrary.Encrypting.Implementation;

namespace MyJournal.Models;

[DataContract]
public class LoginData
{
    [DataMember] public string Login { get; set; }
    [DataMember] public string Password { get; set; }

    public LoginData(string login, string password)
    {
        Login = login;
        Password = password;
    }
}