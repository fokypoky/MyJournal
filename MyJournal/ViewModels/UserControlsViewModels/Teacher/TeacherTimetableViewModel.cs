using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using MyJournal.ViewModels.Base;
using MyJournalLibrary.Entities;
using System.Linq;
using MyJournal.Models;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournal.ViewModels.UserControlsViewModels.Teacher;

public class TeacherTimetableViewModel : ViewModel
{
    private DateTime[] _datesOfWeek = new DateTime[7];

    private List<Timetable> _timetable = new List<Timetable>();

    public List<Timetable> MondayTimetable
    {
        get => GetTimetableByDay(1);
    }

    public List<Timetable> TuesdayTimetable
    {
        get => GetTimetableByDay(2);
    }

    public List<Timetable> WednesdayTimetable
    {
        get => GetTimetableByDay(3);
    }

    public List<Timetable> ThursdayTimetable
    {
        get => GetTimetableByDay(4);
    }

    public List<Timetable> FridayTimetable
    {
        get => GetTimetableByDay(5);
    }

    public List<Timetable> SaturdayTimetable
    {
        get => GetTimetableByDay(6);
    }

    public List<Timetable> SundayTimetable
    {
        get => GetTimetableByDay(7);
    }
    public string TodayInfo
    {
        get => GetDateInfo(DateTime.Now);
    }

    public string MondayInfo
    {
        get => GetDateInfo(_datesOfWeek[0]);
    }

    public string TuesdayInfo
    {
        get => GetDateInfo(_datesOfWeek[1]);
    }

    public string WednesdayInfo
    {
        get => GetDateInfo(_datesOfWeek[2]);
    }

    public string ThursdayInfo
    {
        get => GetDateInfo(_datesOfWeek[3]);
    }

    public string FridayInfo
    {
        get => GetDateInfo(_datesOfWeek[4]);
    }

    public string SaturdayInfo
    {
        get => GetDateInfo(_datesOfWeek[5]);
    }

    public string SundayInfo
    {
        get => GetDateInfo(_datesOfWeek[6]);
    }
    private string DayLanguageFormatter(DateTime date)
    {
        string day = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetDayName(date.DayOfWeek);

        StringBuilder builder = new StringBuilder(day);

        if (char.IsLower(builder[0]))
        {
            builder[0] = char.ToUpper(builder[0]);
        }

        return builder.ToString();
    }

    private List<Timetable> GetTimetableByDay(int day) =>
        (from t in _timetable where t.DayOfWeek == day select t).ToList();
    private string GetDateInfo(DateTime date)
    {
        return $"{DayLanguageFormatter(date)}, {date.Day.ToString()}, {CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetMonthName(date.Month)}";
    }
    public TeacherTimetableViewModel()
    {
        // заполняем дни недели
        int dayOfWeekNow = (int)DateTime.Today.DayOfWeek;

        var date = DateTime.Now.Subtract(new TimeSpan(dayOfWeekNow - 1, 0, 0, 0));

        for (int i = 0; i < 7; i++)
        {
            _datesOfWeek[i] = date.AddDays(i);
        }

        using (var context = new ApplicationContext())
        {
            TimetableRepository timetableService = new TimetableRepository(context);
            var employeeService = new EmployeesRepository(context);

            var employee = employeeService.GetByContactId(ApplicationData.UserId);
            _timetable = timetableService.GetTimetableByEmployee(employee).ToList();
        }

    }
}