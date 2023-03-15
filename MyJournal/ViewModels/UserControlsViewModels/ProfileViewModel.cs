using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using MyJournal.Models;
using MyJournal.Models.Services;
using MyJournal.ViewModels.Base;
using MyJournalLibrary.Entities;

namespace MyJournal.ViewModels.ControlsViewModels;

public class ProfileViewModel : ViewModel
{
    private string _personName;
    private string _email;
    private string _phone;
    private string _password;

    private ICollection<Class> _classes;

    public ICollection<Class> Classes
    {
        get => _classes;
        set => SetField(ref _classes, value);
    }

    public string Email
    {
        get => _email;
        set => SetField(ref _email, value);
    }

    public string Phone
    {
        get => _phone;
        set => SetField(ref _phone, value);
    }

    public string Password
    {
        get => _password;
        set => SetField(ref _password, value);
    }
    public string PersonName
    {
        get => _personName;
        set => SetField(ref _personName, value);
    }

    public ProfileViewModel()
    {
        var contacts = (new ContactService(new ApplicationContext()).GetById(ApplicationData.UserId));
        var employee = (new EmployeeService(new ApplicationContext())).GetByContacts(contacts);

        PersonName = $"{contacts?.Surname} {contacts?.Name} {contacts?.Midname}";
        Email = contacts.Email;
        Phone = contacts.PhoneNumber;
        Password = contacts.Password;
        
        //не работает
        Classes = (new ClassService(new ApplicationContext())).GetClassesByEmployee(employee);
        MessageBox.Show(Classes.Count.ToString());
    }
}