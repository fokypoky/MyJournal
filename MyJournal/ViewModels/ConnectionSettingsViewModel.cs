using System.Diagnostics;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using MyJournal.Models;
using MyJournal.ViewModels.Base;
using System.Linq;
using System.Windows;
using MyJournal.Models.Repositories;

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

    public RelayCommand<Window> ApplyButtonClick
    {
        get => new RelayCommand<Window>(OnApplyButtonClick);
    }

    private void OnApplyButtonClick(Window window)
    {
        string connectionString = $"Host={Host};Port={Port};Username={Username};Password={Password};Database={Database}";
        ApplicationContext.ConnectionString = connectionString;

        DataBaseConnectionRepository repository = new DataBaseConnectionRepository(connectionString);
        repository.Save();

        MessageBox.Show("Новые настройки применены. Перезапустите приложение");
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