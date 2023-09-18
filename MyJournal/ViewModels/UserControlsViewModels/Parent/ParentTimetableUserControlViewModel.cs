using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyJournal.Models;
using MyJournal.ViewModels.Base;
using MyJournalLibrary.Entities;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournal.ViewModels.UserControlsViewModels.Parent;

public class ParentTimetableUserControlViewModel : ViewModel
{
    private MyJournalLibrary.Entities.Parent _parent;
    private ObservableCollection<MyJournalLibrary.Entities.Student> _students;
    private MyJournalLibrary.Entities.Student _selectedStudent;
    private ObservableCollection<Timetable> _selectedStudentTimetable;

    public MyJournalLibrary.Entities.Student SelectedStudent
    {
        get => _selectedStudent;
        set
        {
	        SetField(ref _selectedStudent, value);
	        UpdateTimetable();
        }
    }

    #region Collections

    public ObservableCollection<MyJournalLibrary.Entities.Student> Students
    {
        get => _students;
        set => SetField(ref _students, value);
    }

    #region Days of week timetable collections

    public IEnumerable<Timetable>? MondayTimetable
    {
	    get => GetTimetableByDayOfWeek(1)?.OrderBy(t => t.LessonTime);
    }

    public IEnumerable<Timetable>? TuesdayTimetable
    {
        get => GetTimetableByDayOfWeek(2)?.OrderBy(t => t.LessonTime);
    }

    public IEnumerable<Timetable>? WednesdayTimetable
    {
        get => GetTimetableByDayOfWeek(3)?.OrderBy(t => t.LessonTime);
    }

    public IEnumerable<Timetable>? ThursdayTimetable
    {
        get => GetTimetableByDayOfWeek(4)?.OrderBy(t => t.LessonTime);
    }

    public IEnumerable<Timetable>? FridayTimetable
    {
        get => GetTimetableByDayOfWeek(5)?.OrderBy(t => t.LessonTime);
    }

    public IEnumerable<Timetable>? SaturdayTimetable
    {
        get => GetTimetableByDayOfWeek(6)?.OrderBy(t => t.LessonTime);
    }

    public IEnumerable<Timetable>? SundayTimetable
    {
        get => GetTimetableByDayOfWeek(7)?.OrderBy(t => t.LessonTime);
    }
    #endregion

	#endregion

	private void UpdateTimetable()
    {
	    if (_selectedStudent is null)
	    {
		    return;
	    }

	    using (var context = new ApplicationContext())
	    {
		    _selectedStudentTimetable = new ObservableCollection<Timetable>(
			    new TimetableRepository(context).GetByStudent(_selectedStudent)
		    );
	    }

        NotifyAllTimetableViews();
    }

	private void NotifyAllTimetableViews()
	{
		OnPropertyChanged(nameof(MondayTimetable));
		OnPropertyChanged(nameof(TuesdayTimetable));
		OnPropertyChanged(nameof(WednesdayTimetable));
		OnPropertyChanged(nameof(ThursdayTimetable));
		OnPropertyChanged(nameof(FridayTimetable));
        OnPropertyChanged(nameof(SaturdayTimetable));
        OnPropertyChanged(nameof(SundayTimetable));
	}

    private IEnumerable<Timetable> GetTimetableByDayOfWeek(int dayOfWeek)
    {
	    if (_selectedStudentTimetable is null)
	    {
		    return null;
	    }
	    return from t in _selectedStudentTimetable where t.DayOfWeek == dayOfWeek select t;
    }

    public ParentTimetableUserControlViewModel()
    {
        using (var context = new ApplicationContext())
        {
	        _parent = new ParentsRepository(context).GetByUserId(ApplicationData.UserId);

            Students = new ObservableCollection<MyJournalLibrary.Entities.Student>(
                new StudentsRepository(context).GetWithContactsByParent(_parent)
            );
        }
    }
}