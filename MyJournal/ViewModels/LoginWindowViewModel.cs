using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using MyJournal.Models;
using MyJournal.Models.Services;
using MyJournal.ViewModels.Base;
using MyJournal.Views;
using MyJournalLibrary.Repositories.FileRepositories;

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
        var service = new LoginService(Login, Password);
        service.FillApplicationData();

        switch (ApplicationData.UserRole)
        {
            case UserRole.Employee:
                TeacherWindow teacherWindow = new TeacherWindow();
                teacherWindow.Show();
                break;
            case UserRole.Parent:
                ParentWindow parentWindow = new ParentWindow();
                parentWindow.Show();
                break;
            case UserRole.Student:
                StudentWindow studentWindow = new StudentWindow();
                studentWindow.Show();
                break;
            default:
                MessageBox.Show("Такого пользователя нет");
                break;
        }
    }

    private bool CanLoginButtonClicked() => true;
    // TODO: сделать проверку входа
    //private bool CanLoginButtonClicked() => (new ApplicationContext()).Database.CanConnect();

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
        JsonRepository<DatabaseConnection> repository = new JsonRepository<DatabaseConnection>("ConnectionSettings.json");
        var connection = repository.ReadFile();
        
        if (connection != null)
        {
            ApplicationContext.ConnectionString = connection.ToConnectionString();
        }
        /*DataBaseConnectionRepository connectionRepository = new DataBaseConnectionRepository();
        DatabaseConnection connection = connectionRepository.Load();
        
        TODO: использовать репозиторий
        
        if (connection != null)
        {
            ApplicationContext.ConnectionString = connection.ToConnectionString();
        }*/
    }
}