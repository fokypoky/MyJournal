using MyJournal.Models;
using MyJournal.ViewModels.Base;
using MyJournalLibrary.Entities;
using MyJournalLibrary.Services;

namespace MyJournal.ViewModels;

public class ProfileSettingsViewModel : ViewModel
{
    private Contact _contact;
    private string _password;

    public string Password
    {
        get => _password;
        set => SetField(ref _password, value);
    }
    public Contact Contact
    {
        get => _contact;
        set => SetField(ref _contact, value);
    }
    public ProfileSettingsViewModel()
    {
        using (var context = new ApplicationContext())
        {
            var service = new ContactsService(context);
            _contact = service.GetById(ApplicationData.UserId);
        }
    }
}