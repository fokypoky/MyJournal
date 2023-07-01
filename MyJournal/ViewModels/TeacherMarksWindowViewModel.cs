using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
    private ObservableCollection<Student> _students;

    private int _selectedYear;
    private int _selectedMonth;
    private ObservableCollection<int> _markYears;
    private ObservableCollection<int> _markMonths;

    private DataTable _marksTable;

    public int SelectedYear
    {
        get => _selectedYear;
        set => SetField(ref _selectedYear, value);
    }
    public int SelectedMonth
    {
        get => _selectedMonth;
        set => SetField(ref _selectedMonth, value);
    }
    public DataView MarksDataView
    {
        get => _marksTable.DefaultView;
    }
    public string WindowTitle
    {
        get => _windowTitle;
        set => SetField(ref _windowTitle, value);
    }

    #region Collections
    public ObservableCollection<Student> Students
    {
        get => _students;
        set => SetField(ref _students, value);
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
    #endregion

    #region Commands
    public ICommand LoadCommand
    {
        get => new RelayCommand((object p) =>
        {
            
        });
    }
    public ICommand AddMarkColumnCommand
    {
        get => new RelayCommand((object parameter) =>
        {
            string todayDate = DateTime.Now.ToString("dd.MM");
            foreach (DataColumn column in _marksTable.Columns)
            {
                if (column.ColumnName == todayDate)
                {
                    MessageBox.Show("Такой столбец уже добавлен");
                    return;
                }
            }
            AddMarkColumn(todayDate);
            OnPropertyChanged("MarksDataView");
        });
    }
    #endregion
    
    #region data initialization
    private void AddMarkColumn(string columnTitle)
    {
        var tempTable = new DataTable();
        for (int i = 0; i < _marksTable.Columns.Count; i++)
        {
            var column = new DataColumn(_marksTable.Columns[i].ColumnName);
            tempTable.Columns.Add(column);
        }

        for (int i = 0; i < _marksTable.Rows.Count; i++)
        {
            var rowValues = new List<string>();

            foreach (var rowValue in _marksTable.Rows[i].ItemArray)
            {
                rowValues.Add(rowValue.ToString());
            }

            var newRow = tempTable.NewRow();

            for (int j = 0; j < rowValues.Count; j++)
            {
                newRow[j] = rowValues[j];
            }

            tempTable.Rows.Add(newRow);
        }

        //var newMarkColumn = new DataColumn(columnTitle);
        tempTable.Columns.Add(new DataColumn(columnTitle));

        _marksTable.Clear();
        _marksTable = null;
        _marksTable = tempTable;
    }
    private void AddMarkColumnsRange(ICollection<string> columnTitles)
    {
        var tempTable = new DataTable();
        for (int i = 0; i < _marksTable.Columns.Count; i++)
        {
            var column = new DataColumn(_marksTable.Columns[i].ColumnName);
            tempTable.Columns.Add(column);
        }

        for (int i = 0; i < _marksTable.Rows.Count; i++)
        {
            var rowValues = new List<string>();

            foreach (var rowValue in _marksTable.Rows[i].ItemArray)
            {
                rowValues.Add(rowValue.ToString());
            }

            var newRow = tempTable.NewRow();

            for (int j = 0; j < rowValues.Count; j++)
            {
                newRow[j] = rowValues[j];
            }

            tempTable.Rows.Add(newRow);
        }

        foreach (var columnTitle in columnTitles)
        {
            if (!_marksTable.Columns.Contains(columnTitle))
            {
                tempTable.Columns.Add(new DataColumn(columnTitle));
            }
        }
        
        _marksTable.Clear();
        _marksTable = null;
        _marksTable = tempTable;
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

    private void LoadInfo()
    {
        WindowTitle = $"{_selectedSubject.SubjectTitle} - {_selectedClass.ClassNumber}";

        using (var context = new ApplicationContext())
        {
            Students = new ObservableCollection<Student>(new StudentsRepository(context).GetByClass(_selectedClass)
                .OrderBy(s => s.Contacts.Surname));
        }

        var dates = (
            from student in Students
            from mark in student.Marks
            where mark.SubjectId == _selectedSubject.Id
            select mark.MarkDate).Distinct();

        MarkYears = new ObservableCollection<int>(
            (from date in dates select date.Year).Distinct()
        );
        MarkMonths = new ObservableCollection<int>(
            (from date in dates select date.Month).Distinct()
        );
        
        InitializeMarksDataTable();
    }

    private void InitializeMarksDataTable()
    {
        _marksTable.Clear();
        _marksTable = new DataTable();

        DataColumn studentsColumn = new DataColumn("Ученики");
        _marksTable.Columns.Add(studentsColumn);

        foreach (var student in Students)
        {
            DataRow studentNameRow = _marksTable.NewRow();
            studentNameRow[0] = $"{student.Contacts.Surname} {student.Contacts.Name} {student.Contacts.Midname}";
            _marksTable.Rows.Add(studentNameRow);
        }

        OnPropertyChanged(nameof(MarksDataView));
    }

    public TeacherMarksWindowViewModel()
    {
        WindowMessanger.MessageSender += OnMessageReceived;
        _marksTable = new DataTable();
    }

    #endregion
}