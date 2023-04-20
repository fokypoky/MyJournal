using System.Diagnostics;
using System.Windows.Input;
using MyJournal.Models;
using MyJournal.ViewModels.Base;
using MyJournal.Infrastructure.Commands;
using System.Linq;
using System.Windows;
using MyJournalLibrary.Repositories.FileRepositories;

namespace MyJournal.ViewModels;

public class ConnectionSettingsViewModel : ViewModel
{
    private string _host;
    private string _port;
    private string _database;
    private string _username;
    private string _password;

    public string Host
    {
        get => _host;
        set => SetField(ref _host, value);
    }

    public string Port
    {
        get => _port;
        set => SetField(ref _port, value);
    }

    public string Database
    {
        get => _database;
        set => SetField(ref _database, value);
    }

    public string Username
    {
        get => _username;
        set => SetField(ref _username, value);
    }
    public string Password
    {
        get => _password;
        set => SetField(ref _password, value);
    }

    public RelayCommand ApplyButtonClick
    {
        get => new RelayCommand((object p) =>
        {
            DatabaseConnection connection = new DatabaseConnection(Host, Port, Username, Password, Database);
            JsonRepository<DatabaseConnection> repository = new JsonRepository<DatabaseConnection>("ConnectionSettings.json");
        
            // TODO: довести до ума, метод не должен принимать параметр
            repository.WriteFile(connection);

            MessageBox.Show("Изменения применены. Перезапустите приложение");
        });
    }

    private void OnApplyButtonClick()
    {
        
    }
    
    public ConnectionSettingsViewModel()
    {
        DatabaseConnection connection = new DatabaseConnection(ApplicationContext.ConnectionString);
        
        Host = connection.Host;
        Port = connection.Port;
        Username = connection.Username;
        Password = connection.Password;
        Database = connection.Database;
    }
}