using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using MyJournal.ViewModels.Base;

namespace MyJournal.ViewModels.ControlsViewModels;

public class ProfileViewModel : ViewModel
{
    private string _personName = "Иванов Иван Иванович";
    private string _email;
    private string _phone;
    private string _password;

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
    }

    public ICommand ChangeButtonClick
    {
        get
        {
            return new RelayCommand(OnChangeButtonClick);
        }
    }

    private void OnChangeButtonClick()
    {
        
    }
}