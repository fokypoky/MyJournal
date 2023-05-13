using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
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
    private List<Mark> _marks;
    
    public string WindowTitle
    {
        get => _windowTitle;
        set => SetField(ref _windowTitle, value);
    }

    public List<Mark> Marks
    {
        get => _marks;
        set => SetField(ref _marks, value);
    }
    
    private void LoadInfo()
    {
        using (var context = new ApplicationContext())
        {
            Marks = new MarksRepository(context)
                .GetMarksByClassAndSubject(_selectedClass, _selectedSubject)
                .ToList();
        }

        MessageBox.Show(Marks?.Count.ToString());
    }
    private void OnMessageReceived(object sender, EventArgs e)
    {
        if (e is ClassSubjectMessage)
        {
            _selectedClass = ((ClassSubjectMessage)e).Class;
            _selectedSubject = ((ClassSubjectMessage)e).Subject;

            WindowTitle = $"{_selectedSubject.SubjectTitle} - {_selectedClass.ClassNumber}";
            WindowMessanger.MessageSender -= OnMessageReceived;
            
            LoadInfo();
        }
    }
    
    public TeacherMarksWindowViewModel()
    {
        WindowMessanger.MessageSender += OnMessageReceived;
    }
}