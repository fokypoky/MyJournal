using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
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

    private List<DateTime> _markDates;

    private DataTable _marksTable;

    private int _selectedRowIndex;
    private int _selectedColumnIndex;

    private DataGridCellInfo _cellInfo;

    public DataGridCellInfo CellInfo
    {
        get => _cellInfo;
        set
        {
            SetField(ref _cellInfo, value);
        }
    }
    
    public int SelectedYear
    {
        get => _selectedYear;
        set
        {
            SetField(ref _selectedYear, value);
            OnDatePeriodChanged();
        }
    }
    public int SelectedMonth
    {
        get => _selectedMonth;
        set
        {
            SetField(ref _selectedMonth, value);
            OnDatePeriodChanged();
        }
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
    
    public ICommand OnMarksTableCellEditedCommand
    {
        get => new RelayCommand((object parameter) =>
        {
            if (parameter != null)
            {
                //var newValueList = (DataRowView)parameter; // target value at 0 position
                //string newValue = newValueList[0].ToString();
                var rw = (DataRowView)CellInfo.Item;
                MessageBox.Show(rw[0].ToString());
                return;
                var info = (DataGridCellInfo)parameter;
                var row = (DataRowView)info.Item;
                row.EndEdit();

                string newValue = row[0].ToString();
                
                //_marksTable.Rows[_selectedRowIndex][_selectedColumnIndex] = newValueList[0].ToString();
                _marksTable.Rows[_selectedRowIndex][_selectedColumnIndex] = newValue;
                OnPropertyChanged(nameof(MarksDataView));
            }
        });
    }

    public ICommand OnCellColumnChanged
    {
        get => new RelayCommand((object parameter) =>
        {
            for (int i = 0; i < _marksTable.Columns.Count; i++)
            {
                if (_marksTable.Columns[i].ColumnName == parameter)
                {
                    _selectedColumnIndex = i;
                }
            }
        });
    }
    public ICommand OnCellRowChanged
    {
        get => new RelayCommand((object parameter) =>
        {
            if (parameter != null)
            {
                MessageBox.Show(parameter.ToString());
                _selectedRowIndex = (int)parameter;
            }
        });
    }
    

    public ICommand OnMarksTableSelectedCellChanged
    {
        get => new RelayCommand((object parameter) =>
        {
            MessageBox.Show("sdfsdf");
        });
    }
    public ICommand AddMarkColumnCommand
    {
        get => new RelayCommand((object parameter) =>
        {
            /*if (DateTime.Now.Month != SelectedMonth && DateTime.Now.Year != SelectedYear)
            {
                return;
            }
            */
            
            string todayDate = DateTime.Now.ToString("dd");
            foreach (DataColumn column in _marksTable.Columns)
            {
                if (column.ColumnName == todayDate)
                {
                    MessageBox.Show("Такой столбец уже добавлен");
                    return;
                }
            }
            AddMarkTableColumn(todayDate);
            OnPropertyChanged("MarksDataView");
        });
    }
    #endregion
    
    #region data initialization
    private void AddMarkTableColumn(string columnTitle)
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

        _markDates = (
            from student in Students
            from mark in student.Marks
            where mark.SubjectId == _selectedSubject.Id
            select mark.MarkDate).Distinct().ToList();

        MarkYears = new ObservableCollection<int>(
            (from date in _markDates select date.Year).Distinct()
        );
        MarkMonths = new ObservableCollection<int>(
            (from date in _markDates select date.Month).Distinct()
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

    private void OnDatePeriodChanged()
    {
        if (SelectedMonth == 0 || SelectedYear == 0)
        {
            return;
        }
        FillMarksTable();
        OnPropertyChanged(nameof(MarksDataView));
    }

    private void FillMarksTable()
    {
        _marksTable.Rows.Clear();
        _marksTable.Columns.Clear();
        
        var selectedPeriodDates = (
            from date in _markDates
            where date.Year == SelectedYear && date.Month == SelectedMonth
            select date
        );
        
        AddMarkTableColumn("Ученики");
        
        var columnNames = new List<string>();
        
        foreach (var date in selectedPeriodDates)
        {
            columnNames.Add(date.ToString("dd"));
        }
        AddMarkColumnsRange(columnNames);

        foreach (var student in Students)
        {
            var row = _marksTable.NewRow();
            for (int i = 0; i < _marksTable.Columns.Count; i++)
            {
                if (i == 0)
                {
                    row[i] = $"{student.Contacts.Surname} {student.Contacts.Name} {student.Contacts.Midname}";
                    continue;
                }

                row[i] = student.Marks
                    .Where(m => m.SubjectId == _selectedSubject.Id && 
                                m.MarkDate.Year == SelectedYear && m.MarkDate.Month == SelectedMonth
                                && m.MarkDate.Day == Convert.ToInt32(_marksTable.Columns[i].ColumnName))
                    .FirstOrDefault().MarkValue.ToString();
            }
            
            _marksTable.Rows.Add(row);
        }
    }
    public TeacherMarksWindowViewModel()
    {
        WindowMessanger.MessageSender += OnMessageReceived;
        _marksTable = new DataTable();
    }

    #endregion
}