using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using MyJournal.Models;
using MyJournal.ViewModels.Base;
using MyJournal.Views;

namespace MyJournal.ViewModels;

public class LoginWindowViewModel : ViewModel
{
    private string _login;
    private string _password;
    
    public string Login
    {
        get => _login;
        set => SetField(ref _login, value);
    }

    public string Password
    {
        get => _password;
        set => SetField(ref _password, value);
    }

    public ICommand LoginButtonClick
    {
        get => new RelayCommand(OnLoginButtonClick, CanLoginButtonClicked);
    }
    private void OnLoginButtonClick()
    {
        TeacherWindow window = new TeacherWindow();
        window.Show();
    }
    private bool CanLoginButtonClicked() => (new ApplicationContext()).Database.CanConnect();

    public ICommand ConnectionSettingsButtonClick
    {
        get => new RelayCommand(() =>
        {
            ConnectionSettingsWindow settingsWindow = new ConnectionSettingsWindow();
            settingsWindow.Show();
        });
    }
    
    public LoginWindowViewModel()
    {
        
    }
}