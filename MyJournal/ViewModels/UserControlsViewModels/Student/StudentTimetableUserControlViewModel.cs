using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyJournal.Models;
using MyJournal.ViewModels.Base;
using MyJournalLibrary.Entities;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournal.ViewModels.UserControlsViewModels.Student;

public class StudentTimetableUserControlViewModel : ViewModel
{
    private MyJournalLibrary.Entities.Student _student;
    private ObservableCollection<Timetable> _studentTimetable;

    #region Day collections public fields
    public IEnumerable<Timetable> MondayList
    {
        get => GetTimetableByDayNumber(1).OrderBy(t => t.LessonTime);
    }

    public IEnumerable<Timetable> TuesdayList
    {
        get => GetTimetableByDayNumber(2).OrderBy(t => t.LessonTime);
    }

    public IEnumerable<Timetable> WednesdayList
    {
        get => GetTimetableByDayNumber(3).OrderBy(t => t.LessonTime);
    }

    public IEnumerable<Timetable> ThursdayList
    {
        get => GetTimetableByDayNumber(4).OrderBy(t => t.LessonTime);
    }

    public IEnumerable<Timetable> FridayList
    {
        get => GetTimetableByDayNumber(5).OrderBy(t => t.LessonTime);
    }

    public IEnumerable<Timetable> SaturdayList
    {
        get => GetTimetableByDayNumber(6).OrderBy(t => t.LessonTime);
    }

    public IEnumerable<Timetable> SundayList
    {
        get => GetTimetableByDayNumber(7).OrderBy(t => t.LessonTime);
    }
    #endregion

    public string CurrentDate
    {
        get
        {
            string currentDayOfWeekName =
                CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek);

            string currentMonthName =
                CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetMonthName(DateTime.Now.Month);

            return $"{currentDayOfWeekName}, {DateTime.Now.Day}, {currentMonthName}";
        }
    }
    #region Collections
    public ObservableCollection<Timetable> StudentTimetable
    {
        get => _studentTimetable;
        set => SetField(ref _studentTimetable, value);
    }
    #endregion

    private IEnumerable<Timetable> GetTimetableByDayNumber(int dayNumber)
    {
        return from t in _studentTimetable where t.DayOfWeek == dayNumber select t;
    }
    public StudentTimetableUserControlViewModel()
    {
        using (var context = new ApplicationContext())
        {
            var studentsRepository = new StudentsRepository(context);
            var timetableRepository = new TimetableRepository(context);

            _student = studentsRepository.GetById(ApplicationData.UserId);
            StudentTimetable = new ObservableCollection<Timetable>(
                timetableRepository.GetByStudent(_student)
            );
        }
    }
}