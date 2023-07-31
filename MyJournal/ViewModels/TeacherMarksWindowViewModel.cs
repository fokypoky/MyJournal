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
    private List<int> _aviableMarkValues = new List<int>() { 5, 4, 3, 2 };
    private int _selectedNewMarkValue;

    private int _selectedYear;
    private int _selectedMonth;
    private ObservableCollection<int> _markYears;
    private ObservableCollection<int> _markMonths;

    private List<DateTime> _markDates;
    private DataTable _marksTable;
    private int _selectedCellRowIndex;

    private DataGridCellInfo _selectedCellInfo;

    private ObservableCollection<Task> _currentPeriodTasks;

    #region Public Fields
    public int SelectedCellRowIndex
    {
        get => _selectedCellRowIndex;
        set => SetField(ref _selectedCellRowIndex, value);
    }
    public int SelectedNewMarkValue
    {
        get => _selectedNewMarkValue;
        set => SetField(ref _selectedNewMarkValue, value);
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
    #endregion
    #region Collections

    public ObservableCollection<Task> CurrentPeriodTasks
    {
        get => _currentPeriodTasks;
        set => SetField(ref _currentPeriodTasks, value);
    }
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

    public List<int> AviableMarkValues
    {
        get => _aviableMarkValues;
        set => SetField(ref _aviableMarkValues, value);
    }
    #endregion

    #region Commands

    public ICommand SaveChangesCommand
    {
        get => new RelayCommand((object parameter) =>
        {
            if (_marksTable.Columns.Count <= 1)
            {
                MessageBox.Show("Нет оценок");
                return;
            }

            var aviableMarkValuesString = new List<string>();
            foreach (int value in AviableMarkValues)
            {
                aviableMarkValuesString.Add(value.ToString());
            }

            var newMarks = new List<Mark>();
            var oldMarks = new List<Mark>();

            for (int i = 0; i < _marksTable.Rows.Count; i++) // i - строки
            {
                for (int j = 1; j < _marksTable.Columns.Count; j++) // Columns[0] содержит имена учеников, поэтому int j = 1
                {
                    // проверка валидности оценки

                    string markValueString = _marksTable.Rows[i][j].ToString();
                    if (String.IsNullOrEmpty(markValueString))
                    {
                        continue;
                    }
                    else if (!aviableMarkValuesString.Contains(markValueString))
                    {
                        continue;
                    }

                    int markValue = Convert.ToInt32(markValueString);
                    int currentMarkDay = Convert.ToInt32(_marksTable.Columns[j].ColumnName);

                    DateTime currentMarkDate = new DateTime(SelectedYear, SelectedMonth, currentMarkDay);
                    
                    // проверка существования оценки

                    bool isMarkExists = false;
                    int markIndex = -1;

                    for (int k = 0; k < Students[i].Marks.Count; k++)
                    {
                        var mark = Students[i].Marks.ElementAt(k);
                        if (mark.MarkDate.Date == currentMarkDate.Date)
                        {
                            isMarkExists = true;
                            markIndex = k;
                            break;
                        } 
                    }

                    // изменение оценки
                    if (isMarkExists)
                    {
                        if (Students[i].Marks.ElementAt(markIndex).MarkValue != markValue)
                        {
                            Students[i].Marks.ElementAt(markIndex).MarkValue = markValue;
                            oldMarks.Add(Students[i].Marks.ElementAt(markIndex));
                        }
                    }
                    else
                    {
                        // TODO: сделать нормальную привязку к заданию у mark 
                        var mark = new Mark()
                        {
                            MarkDate = new DateTime(SelectedYear, SelectedMonth, currentMarkDay),
                            MarkValue = markValue,
                            //Student = Students[i],
                            StudentId = Students[i].Id,
                            //Subject = _selectedSubject,
                            SubjectId = _selectedSubject.Id,
                            TeacherId = ApplicationData.UserId
                        };
                        newMarks.Add(mark);
                    }
                }
            }

            using (var context = new ApplicationContext())
            {
                var repository = new MarksRepository(context);

                if (newMarks.Count > 0)
                {
                    repository.AddRange(newMarks);
                }

                if (oldMarks.Count > 0)
                {
                    repository.UpdateRange(oldMarks);
                }
            }
            MessageBox.Show("Изменения сохранены");
        });
    }
    public ICommand OnSelectedCellChanged
    {
        get => new RelayCommand((object parameter) =>
        {
            if (parameter is DataGridCellInfo)
            {
                _selectedCellInfo = (DataGridCellInfo)parameter;
            }
        });
    }
    public ICommand SetSelectedRowIndexCommand
    {
        get => new RelayCommand((object parameter) =>
        {
            if (parameter != null)
            {
                if ((int)parameter < 0)
                {
                    return;
                }
                _selectedCellRowIndex = (int)parameter;
            }
        });
    }
    public ICommand UpdateMarkCommand
    {
        get => new RelayCommand((object parameter) =>
        {
            if (SelectedNewMarkValue == 0)
            {
                MessageBox.Show("Новое значение не выбрано");
                return;
            }

            int selectedCellColumnIndex = 0;
            string selectedCellColumnName = _selectedCellInfo.Column.Header.ToString();

            for (int i = 0; i < _marksTable.Columns.Count; i++)
            {
                if (_marksTable.Columns[i].ColumnName == selectedCellColumnName)
                {
                    selectedCellColumnIndex = i;
                }
            }

            if (selectedCellColumnIndex <= 0 || SelectedCellRowIndex < 0)
            {
                MessageBox.Show("Неверный индекс ячейки");
                return;
            }

            _marksTable.Rows[SelectedCellRowIndex][selectedCellColumnIndex] = SelectedNewMarkValue;
            OnPropertyChanged(nameof(MarksDataView));
        });
    }
    public ICommand AddMarkColumnCommand
    {
        get => new RelayCommand((object parameter) =>
        {
            if (DateTime.Now.Month != SelectedMonth && DateTime.Now.Year != SelectedYear)
            {
                return;
            }


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

        using (var context = new ApplicationContext())
        {
            CurrentPeriodTasks?.Clear();
            CurrentPeriodTasks = new ObservableCollection<Task>(
                new TasksRepository(context).GetByClassSubjectAndPeriod(_selectedClass, _selectedSubject, SelectedYear,
                    SelectedMonth)
            );
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
        ).OrderBy(d => d.Day);

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
            using (var context = new ApplicationContext())
            {
                var marksRepository = new MarksRepository(context);

                for (int i = 0; i < _marksTable.Columns.Count; i++)
                {
                    if (i == 0)
                    {
                        row[i] = $"{student.Contacts.Surname} {student.Contacts.Name} {student.Contacts.Midname}";
                        continue;
                    }

                    var currentMarkDate = new DateTime(SelectedYear, SelectedMonth,
                        Convert.ToInt32(_marksTable.Columns[i].ColumnName));
                    
                    var mark = marksRepository.GetByStudentSubjectAndDate(student, _selectedSubject, currentMarkDate);
                    row[i] = mark?.MarkValue.ToString();
                }

                _marksTable.Rows.Add(row);
            }
        }
    }
    public TeacherMarksWindowViewModel()
    {
        WindowMessanger.MessageSender += OnMessageReceived;
        _marksTable = new DataTable();
    }

    #endregion
}