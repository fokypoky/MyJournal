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
using Task = MyJournalLibrary.Entities.Task;

namespace MyJournal.ViewModels.UserControlsViewModels.Student;

public class StudentTasksUserControl : ViewModel
{
    private MyJournalLibrary.Entities.Student _student;
    private Subject _selectedSubject;
    private int _selectedYear;
    private int _selectedMonth;

    private ObservableCollection<int> _years;
    private ObservableCollection<int> _months;

    private ObservableCollection<Task> _tasks;
    private ObservableCollection<Task> _expiringTasks;
    #region ComboBox selected items
    public Subject SelectedSubject
    {
        get => _selectedSubject;
        set
        {
            SetField(ref _selectedSubject, value);
            LoadYears();
        }
    }

    public int SelectedYear
    {
        get => _selectedYear;
        set
        {
            SetField(ref _selectedYear, value);
            LoadMonth();
        }
    }

    public int SelectedMonth
    {
        get => _selectedMonth;
        set
        {
            SetField(ref _selectedMonth, value);
            LoadTasks();
        }
    }
    #endregion

    #region Collections
    public ObservableCollection<int> Years
    {
        get => _years;
        set => SetField(ref _years, value);
    }

    public ObservableCollection<int> Months
    {
        get => _months;
        set => SetField(ref _months, value);
    }

    public ObservableCollection<Task> Tasks
    {
        get => _tasks;
        set => SetField(ref _tasks, value);
    }

    public ObservableCollection<Task> ExpiringTasks
    {
        get => _expiringTasks;
        set => SetField(ref _expiringTasks, value);
    }
    #endregion

    public MyJournalLibrary.Entities.Student Student
    {
        get => _student;
        set => SetField(ref _student, value);
    }

    private void LoadYears()
    {
        using (var context = new ApplicationContext())
        {
            Years?.Clear();
            Years = new ObservableCollection<int>(
                new TasksRepository(context).GetTaskYearsByClassAndSubject(Student.Class, SelectedSubject)
            );
        }
    }

    private void LoadMonth()
    {
        using (var context = new ApplicationContext())
        {
            Months?.Clear();
            Months = new ObservableCollection<int>(
                new TasksRepository(context).GetTaskMonthByClassSubjectAndYear(Student.Class, SelectedSubject,
                    SelectedYear)
            );
        }
    }

    private void LoadTasks()
    {
        using (var context = new ApplicationContext())
        {
            var tasksRepository = new TasksRepository(context);

            Tasks?.Clear();
            Tasks = new ObservableCollection<Task>(
                tasksRepository.GetByClassSubjectAndPeriod(Student.Class, SelectedSubject, SelectedYear,
                    SelectedMonth));

            ExpiringTasks?.Clear();
            ExpiringTasks = new ObservableCollection<Task>(
                tasksRepository.GetExpiringTasksByClassSubjectAndDate(Student.Class, SelectedSubject, DateTime.Now));
        }
    }

    public StudentTasksUserControl()
    {
        using (var context = new ApplicationContext())
        {
            Student = new StudentsRepository(context).GetByIdWithContactsClassAndSubjects(ApplicationData.UserId);
        }
        Years = new ObservableCollection<int>();
        Months = new ObservableCollection<int>();
        Tasks = new ObservableCollection<Task>();
        ExpiringTasks = new ObservableCollection<Task>();
    }
}