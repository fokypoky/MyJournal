using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using MyJournal.ViewModels.Base;
using MyJournal.ViewModels.Controls;
using MyJournal.Views;

namespace MyJournal.ViewModels;

public class TeacherWindowViewModel : ViewModel
{
    private UserControl _mainUserControl;
    private UserControl _chatUserControl;
    private UserControl _marksUserControl;
    private UserControl _profileUserControl;
    private UserControl _tasksUserControl;
    private UserControl _timetableUserControl;

    private UserControl _currentUserControl;
    
    public UserControl CurrentUserControl
    {
        get => _currentUserControl;
        set => SetField(ref _currentUserControl, value);
    }

    public ICommand MainButtonClick
    {
        get => new RelayCommand(() => { CurrentUserControl = _mainUserControl; });
    }

    public ICommand ChatButtonClick
    {
        get => new RelayCommand(() => { CurrentUserControl = _chatUserControl; });
    }

    public ICommand MarksButtonClick
    {
        get => new RelayCommand(() => { CurrentUserControl = _marksUserControl; });
    }

    public ICommand ProfileButtonClick
    {
        get => new RelayCommand(() => { CurrentUserControl = _profileUserControl; });
    }

    public ICommand TasksButtonClick
    {
        get => new RelayCommand(() => { CurrentUserControl = _tasksUserControl;});
    }

    public ICommand TimetableButtonClick
    {
        get => new RelayCommand(() => { CurrentUserControl = _timetableUserControl; });
    }

    public ICommand ConnectionSettingsButtonClick
    {
        get => new RelayCommand((() =>
        {
            ConnectionSettingsWindow settingsWindow = new ConnectionSettingsWindow();
            settingsWindow.Show();
        }));
    }
    
    public TeacherWindowViewModel()
    {
        _mainUserControl = new MainUserControl();
        _chatUserControl = new ChatUserControl();
        _marksUserControl = new MarksUserControl();
        _profileUserControl = new ProfileUserControl();
        _tasksUserControl = new TasksUserControl();
        _timetableUserControl = new TimetableUserControl();

        _currentUserControl = _mainUserControl;
    }
}