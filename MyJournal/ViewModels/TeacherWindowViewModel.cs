using System.Windows.Controls;
using System.Windows.Input;
using MyJournal.Infrastructure.Commands;
using MyJournal.ViewModels.Base;
using MyJournal.ViewModels.Controls;
using MyJournal.Views.UserControls;

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
        get => new RelayCommand((object parameter) =>
        {
            CurrentUserControl = _mainUserControl;
        });
    }

    public ICommand ChatButtonClick
    {
        get => new RelayCommand((object parameter) =>
        {
            CurrentUserControl = _chatUserControl;
        });
    }

    public ICommand MarksButtonClick
    {
        get => new RelayCommand((object parameter) =>
        {
            CurrentUserControl = _marksUserControl;
        });
    }

    public ICommand ProfileButtonClick
    {
        get => new RelayCommand((object parameter) =>
        {
            CurrentUserControl = _profileUserControl;
        });
    }

    public ICommand TasksButtonClick
    {
        get => new RelayCommand((object parameter) =>
        {
            CurrentUserControl = _tasksUserControl;
        });
    }

    public ICommand TimetableButtonClick
    {
        get => new RelayCommand((object parameter) =>
        {
            CurrentUserControl = _timetableUserControl;
        });
    }
    public TeacherWindowViewModel()
    {
        _mainUserControl = new MainUserControl();
        _chatUserControl = new ChatUserControl();
        _marksUserControl = new TeacherSelectionMarksUserControl();
        _profileUserControl = new ProfileUserControl();
        _tasksUserControl = new TasksUserControl();
        _timetableUserControl = new TeacherTimetableUserControl();

        _currentUserControl = _mainUserControl;
    }
}