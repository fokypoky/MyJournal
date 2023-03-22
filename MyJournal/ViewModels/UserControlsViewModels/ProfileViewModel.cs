using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using MyJournal.Models;
using MyJournal.ViewModels.Base;
using MyJournalLibrary.Entities;
using MyJournalLibrary.Services;

namespace MyJournal.ViewModels.ControlsViewModels;

public class ProfileViewModel : ViewModel
{
    private string _personName;
    private string _email;
    private string _phone;

    private ICollection<Subject> _subjects = new List<Subject>();
    private ICollection<Class> _classes = new List<Class>();

    public ICollection<Subject> Subjects
    {
        get => _subjects;
        set => SetField(ref _subjects, value);
    }

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
    
    public string PersonName
    {
        get => _personName;
        set => SetField(ref _personName, value);
    }

    public ProfileViewModel()
    {
        Employee employee;
        
        using (var context = new ApplicationContext())
        {
            var service = new EmployeesService(context);
            employee = service.GetByContactId(ApplicationData.UserId);
        }
        
        PersonName = $"{employee.Contacts.Surname} {employee.Contacts.Name} {employee.Contacts.Midname}";
        Email = employee.Contacts.Email;
        Phone = employee.Contacts.PhoneNumber;

        Subjects = employee.Subjects;
        Classes = employee.Classes;
    }
}