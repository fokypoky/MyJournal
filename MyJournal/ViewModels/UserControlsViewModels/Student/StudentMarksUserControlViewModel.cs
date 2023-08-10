using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using MyJournal.Models;
using MyJournal.ViewModels.Base;
using MyJournalLibrary.Entities;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournal.ViewModels.UserControlsViewModels.Student;

public class StudentMarksUserControlViewModel : ViewModel
{
    private MyJournalLibrary.Entities.Student _student;
    
    private Subject _selectedSubject;
    private int _selectedYear;
    private int _selectedMonth;

    private ObservableCollection<int> _years;
    private ObservableCollection<int> _months;

    private ObservableCollection<Mark> _marks;

    public MyJournalLibrary.Entities.Student Student
    {
        get => _student;
        set => SetField(ref _student, value);
    }

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
            LoadMarks();
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

    public ObservableCollection<Mark> Marks
    {
        get => _marks;
        set => SetField(ref _marks, value);
    }
    #endregion
    
    private void LoadYears()
    {
        using (var context = new ApplicationContext())
        {
            Years?.Clear();
            Years = new ObservableCollection<int>(
                new MarksRepository(context).GetMarkYearsByStudentAndSubject(Student, SelectedSubject).Order()
            );
        }
    }

    private void LoadMonth()
    {
        using (var context = new ApplicationContext())
        {
            Months?.Clear();
            Months = new ObservableCollection<int>(
                new MarksRepository(context).GetMarkMonthsByStudentSubjectAndYear(Student, SelectedSubject, SelectedYear)
                    .Order()
            );
        }
    }

    private void LoadMarks()
    {
        using (var context = new ApplicationContext())
        {
            Marks?.Clear();
            Marks = new ObservableCollection<Mark>(
                new MarksRepository(context).GetWithTeacherAndContactsByStudentSubjectAndPeriod(Student,
                    SelectedSubject, SelectedYear, SelectedMonth).OrderByDescending(m => m.MarkDate)
            );
        }
    }
    public StudentMarksUserControlViewModel()
    {
        using (var context = new ApplicationContext())
        {
            Student = new StudentsRepository(context).GetByIdWithContactsClassAndSubjects(ApplicationData.UserId);
        }    
    }
}