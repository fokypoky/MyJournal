using System.Linq;
using System.Runtime.Serialization;


namespace MyJournalAdmin.Models
{
	[DataContract]
    public class DatabaseConnection
    {
	    [DataMember]
	    public string Host { get; set; }
	    [DataMember]
	    public string Port { get; set; }
	    [DataMember]
	    public string Username { get; set; }
	    [DataMember]
	    public string Password { get; set; }
	    [DataMember]
	    public string Database { get; set; }

	    public DatabaseConnection(string host, string port, string username, string password, string database)
	    {
		    Host = host;
		    Port = port;
		    Username = username;
		    Password = password;
		    Database = database;
	    }
	    public DatabaseConnection(string connectionString)
	    {
		    Host = GetPropertyFromConnectionString(connectionString, "host");
		    Port = GetPropertyFromConnectionString(connectionString, "port");
		    Username = GetPropertyFromConnectionString(connectionString, "username");
		    Password = GetPropertyFromConnectionString(connectionString, "password");
		    Database = GetPropertyFromConnectionString(connectionString, "database");
	    }

	    private string GetPropertyFromConnectionString(string connectionString, string propertyName)
	    {
		    var connectionParameters = from p in connectionString.Replace(" ", "").Split(';')
			    select p.Split('=');
		    return (from p in connectionParameters
			    where p.ElementAt(0).ToLower() == propertyName.ToLower()
			    select p.ElementAt(1)).ElementAt(0);
	    }

	    public string ToConnectionString() => $"Host={Host};Port={Port};Username={Username};Password={Password};Database={Database}";

	}
}
