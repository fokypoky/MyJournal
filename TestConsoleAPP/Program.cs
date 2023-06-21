using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using MyJournalLibrary.Encrypting;
using MyJournalLibrary.Encrypting.Implementation;
using MyJournalLibrary.Encrypting.Keys.Implementation;

namespace TestConsoleAPP;

class Program
{
    public static void Main(string[] args)
    {
        //string text = "HELLO WORLD";
        //byte[] key = new byte[32];
        //new Random().NextBytes(key);

        //byte[] iv = new byte[16];
        //new Random().NextBytes(iv);

        //AESKey aesKey = new AESKey(key, iv);

        //AESEncryptor encryptor = new AESEncryptor(aesKey);

        //var encrypted = encryptor.Encrypt(text);

        //Console.WriteLine(encryptor.Decrypt(encrypted));

        AESKey key = new AESKey(
            Encoding.UTF8.GetBytes("MyJournalAESKey"),
            new byte[] { 142, 11, 13, 188, 138, 80, 185, 126, 5, 27, 33, 33, 75, 18, 210, 232 }
        );
        var _encryptor = new AESEncryptor(key);

        var loginData = new LoginData("admin", "admsfsdjfnsdfin");
        string login = Convert.ToBase64String(_encryptor.Encrypt(loginData.Login));
        string password = Convert.ToBase64String(_encryptor.Encrypt(loginData.Password));

        Console.WriteLine(login);
        Console.WriteLine(password);

        string dlogin = _encryptor.Decrypt(Convert.FromBase64String(login));
        string dpassword = _encryptor.Decrypt(Convert.FromBase64String(password));

        Console.WriteLine(dlogin);
        Console.WriteLine(dpassword);

    }
}
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