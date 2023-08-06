using System.Windows.Controls;
using System.Windows.Input;
using MyJournal.Infrastructure.Commands;
using MyJournal.Models;
using MyJournal.Models.Builders.Directors;
using MyJournal.ViewModels.Base;
using MyJournal.ViewModels.Controls;
using MyJournal.Views.UserControls;
using MyJournal.Views.UserControls.Teacher;

namespace MyJournal.ViewModels;

public class MainWindowViewModel : ViewModel
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
    public MainWindowViewModel()
    {
        MainWindowUserControlsBuilderDirector director =
            new MainWindowUserControlsBuilderDirector(ApplicationData.UserRole);
        director.Construct();

        _mainUserControl = director.Builder.MainUserControl;
        _chatUserControl = director.Builder.ChatUserControl;
        _marksUserControl = director.Builder.MarksUserControl;
        _profileUserControl = director.Builder.ProfileUserControl;
        _tasksUserControl = director.Builder.TasksUserControl;
        _timetableUserControl = director.Builder.TimeTableUserControl;

        _currentUserControl = _mainUserControl;
    }
}