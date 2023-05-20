using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MyJournal.Infrastructure.Commands;
using MyJournal.Models;
using MyJournal.ViewModels.Base;
using MyJournalLibrary.Entities;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournal.ViewModels;

public class TeacherMarksWindowViewModel : ViewModel
{
    private string _windowTitle;
    private Class _selectedClass;
    private Subject _selectedSubject;
    private ObservableCollection<Mark> _marks;
    
    private ObservableCollection<int> _markYears;
    private int _selectedYear;

    private ObservableCollection<int> _markMonths;
    private int _selectedMonth;
    
    public int SelectedYear
    {
        get => _selectedYear;
        set
        {
            SetField(ref _selectedYear, value);
        }
    }

    public int SelectedMonth
    {
        get => _selectedMonth;
        set
        {
            SetField(ref _selectedMonth, value);
        }
    }
    
    public string WindowTitle
    {
        get => _windowTitle;
        set => SetField(ref _windowTitle, value);
    }

    public ObservableCollection<Mark> Marks
    {
        get => _marks;
        set => SetField(ref _marks, value);
    }

    public ObservableCollection<int> MarkYears
    {
        get => _markYears;
        set => SetField(ref _markYears, value);
    }

    public ObservableCollection<int> MarkMonths
    {
        get => _markMonths;
        set => SetField(ref _markMonths, value);
    }

    #region Commands

    public ICommand OnYearComboBoxItemChanged
    {
        get => new RelayCommand((object parameter) =>
        {
            MessageBox.Show("OK");
        });
    }
    #endregion

    #region data initialization
    private void LoadInfo()
    {
        WindowTitle = $"{_selectedSubject.SubjectTitle} - {_selectedClass.ClassNumber}";
        using (var context = new ApplicationContext())
        {
            Marks =  new ObservableCollection<Mark>
            ( 
                new MarksRepository(context)
                    .GetMarksByClassAndSubject(_selectedClass, _selectedSubject)
                    .ToList()
            );
        }

        MarkYears = new ObservableCollection<int>((from mark in Marks select mark.MarkDate.Year).ToList().Distinct());
        SelectedYear = MarkYears.Max();
        
        MarkMonths = new ObservableCollection<int>((from mark in Marks where mark.MarkDate.Year == SelectedYear
            select mark.MarkDate.Month).ToList().Distinct());
    }
    private void OnMessageReceived(object sender, EventArgs e)
    {
        if (e is ClassSubjectMessage)
        {
            _selectedClass = ((ClassSubjectMessage)e).Class;
            _selectedSubject = ((ClassSubjectMessage)e).Subject;
            
            WindowMessanger.MessageSender -= OnMessageReceived;
            
            LoadInfo();
        }
    }
    
    public TeacherMarksWindowViewModel()
    {
        WindowMessanger.MessageSender += OnMessageReceived;
    }

    #endregion
}