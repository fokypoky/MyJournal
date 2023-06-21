using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using MyJournal.Models;
using MyJournal.ViewModels.Base;
using MyJournal.Views;
using MyJournalLibrary.Repositories.FileRepositories;
using UserRole = MyJournal.Models.UserRole;
using MyJournal.Infrastructure.Commands;
using MyJournalLibrary.Encrypting;
using MyJournalLibrary.Encrypting.Implementation;
using MyJournalLibrary.Encrypting.Keys.Implementation;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournal.ViewModels;

public class LoginWindowViewModel : ViewModel
{
    private string _login;
    private string _password;
    private bool _needsToSaveLogin = false;
    private IEncryptor _encryptor;
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

    public bool NeedsToSaveLogin
    {
        get => _needsToSaveLogin;
        set => SetField(ref _needsToSaveLogin, value);
    }

    public ICommand RememberLogin
    {
        get => new RelayCommand((object parameter) =>
        {
            NeedsToSaveLogin = !NeedsToSaveLogin;
        });
    }
    public ICommand LoginButtonClick
    {
        get => new RelayCommand(OnLoginButtonClick, CanLoginButtonClicked);
    }
    
    private void OnLoginButtonClick(object parameter)
    {
        using (var db = new ApplicationContext())
        {
            var service = new ContactsRepository(db);
            var contact = service.GetByLogin(Login, Password);

            if (contact == null)
            {
                MessageBox.Show("Такого пользователя не существует");
                return;
            }
            
            ApplicationData.UserId = contact.Id;
            bool needToClose = false; // нужно ли закрывать окно
            
            switch (contact.UserRole.Rolename.ToLower())
            {
                case "employee":
                    ApplicationData.UserRole = UserRole.Employee;
                    
                    TeacherWindow teacherWindow = new TeacherWindow();
                    teacherWindow.Show();
                    
                    needToClose = true;
                    
                    break;
                case "student":
                    ApplicationData.UserRole = UserRole.Student;
                    
                    StudentWindow studentWindow = new StudentWindow();
                    studentWindow.Show();
                    
                    needToClose = true;
                    
                    break;
                case "parent":
                    ApplicationData.UserRole = UserRole.Student;

                    ParentWindow parentWindow = new ParentWindow();
                    parentWindow.Show();
                    
                    needToClose = true;
                    
                    break;
                default:
                    ApplicationData.UserRole = UserRole.None;
                    MessageBox.Show("Недостаточно привилегий");
                    break;
            }

            if (NeedsToSaveLogin)
            {
                //string login = _encryptor.Encrypt(Login);
                //string password = _encryptor.Encrypt(Password);
                string encryptedLogin = Convert.ToBase64String(_encryptor.Encrypt(Login));
                string encryptedPassword = Convert.ToBase64String(_encryptor.Encrypt(Password));

                var repository = new JsonRepository<LoginData>("LoginData.json");
                repository.WriteFile(new LoginData(encryptedLogin, encryptedPassword));
            }
            if (needToClose)
            {
                Application.Current.MainWindow.Close();
            }
            
        }
    }

    private bool CanLoginButtonClicked(object parameter)
    {
        using (var db = new ApplicationContext())
        {
            return db.Database.CanConnect();
        }
    }

    public ICommand ConnectionSettingsButtonClick
    {
        get => new RelayCommand((object parameter) =>
        {
            ConnectionSettingsWindow settingsWindow = new ConnectionSettingsWindow();
            settingsWindow.Show();
        });
    }
    public LoginWindowViewModel()
    {
        ApplicationData.AESKey = new AESKey(
            Encoding.UTF8.GetBytes("MyJournalAESKey"),
            new byte[] { 142, 11, 13, 188, 138, 80, 185, 126, 5, 27, 33, 33, 75, 18, 210, 232}
        );
        _encryptor = new AESEncryptor(ApplicationData.AESKey);
        
        JsonRepository<DatabaseConnection> repository = new JsonRepository<DatabaseConnection>("ConnectionSettings.json");
        var connection = repository.ReadFile();
        
        if (connection != null)
        {
            ApplicationContext.ConnectionString = connection.ToConnectionString();
        }

        if (File.Exists("LoginData.json"))
        {
            JsonRepository<LoginData> loginRepository = new JsonRepository<LoginData>("LoginData.json");
            var loginData = loginRepository.ReadFile();

            Login = _encryptor.Decrypt(Convert.FromBase64String(loginData.Login));
            Password = _encryptor.Decrypt(Convert.FromBase64String(loginData.Password));
            //Login = _encryptor.Decrypt(loginData.Login);
            //Password = _encryptor.Decrypt(loginData.Password);
        }
    }
}