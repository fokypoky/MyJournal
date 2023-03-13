using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using MyJournal.ViewModels.Base;

namespace MyJournal.ViewModels;

public class ConnectionSettingsViewModel : ViewModel
{
    private string _host = "localhost";
    private string _port = "5432";
    private string _database = "MyJournalDB";
    private string _username = "postgres";
    private string _password = "toor";

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

    public ICommand ApplyButtonClick
    {
        get => new RelayCommand(OnApplyButtonClick);
    }

    private void OnApplyButtonClick()
    {
        
    }
}