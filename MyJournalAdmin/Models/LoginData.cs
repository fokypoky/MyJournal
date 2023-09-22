using System.Runtime.Serialization;

namespace MyJournalAdmin.Models
{
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
}
