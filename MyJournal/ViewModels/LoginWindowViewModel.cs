﻿using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using MyJournal.Models;
using MyJournalLibrary.Services;
using MyJournal.ViewModels.Base;
using MyJournal.Views;
using MyJournalLibrary.Entities;
using MyJournalLibrary.Repositories.FileRepositories;
using UserRole = MyJournal.Models.UserRole;

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
        using (var db = new ApplicationContext())
        {
            var service = new ContactsService(db);
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

            if (needToClose)
            {
                Application.Current.MainWindow.Close();
            }
            
        }
    }

    private bool CanLoginButtonClicked()
    {
        using (var db = new ApplicationContext())
        {
            return db.Database.CanConnect();
        }
    }
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
    }
}