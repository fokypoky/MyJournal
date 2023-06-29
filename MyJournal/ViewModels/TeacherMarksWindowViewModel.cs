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

    private DataTable _marksTable;

    public DataView MarksDataView
    {
        get => _marksTable.DefaultView;
        set => _marksTable = value.ToTable();
    }

    public ObservableCollection<Student> Students
    {
        get => _students;
        set => SetField(ref _students, value);
    }

    public string WindowTitle
    {
        get => _windowTitle;
        set => SetField(ref _windowTitle, value);
    }

    #region data initialization

    public ICommand LoadCommand
    {
        get => new RelayCommand((object p) =>
        {
            //_marksTable.Clear();
            //_marksTable = new DataTable();

            //DataColumn studentsColumn = new DataColumn();
            //studentsColumn.ColumnName = "Ученики";

            //_marksTable.Columns.Add(studentsColumn);

            //foreach (var student in Students)
            //{
            //    DataRow row = _marksTable.NewRow();

            //    string studentName = $"{student.Contacts.Surname} {student.Contacts.Name} {student.Contacts.Midname}";

            //    row[0] = studentName;

            //    _marksTable.Rows.Add(row);
            //}

            //OnPropertyChanged("MarksDataView");
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

            var newMarkColumn = new DataColumn(todayDate);
            tempTable.Columns.Add(newMarkColumn);

            _marksTable.Clear();
            _marksTable = null;
            _marksTable = tempTable;

            OnPropertyChanged("MarksDataView");
        });
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