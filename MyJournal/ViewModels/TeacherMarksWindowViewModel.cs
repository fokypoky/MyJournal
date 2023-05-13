using System;
using System.Collections.ObjectModel;
using System.Linq;
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
    private ObservableCollection<Mark> _marks = new ObservableCollection<Mark>();
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
    
    #region Commands

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