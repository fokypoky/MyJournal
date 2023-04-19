using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using MyJournal.Models;
using MyJournal.ViewModels.Base;
using MyJournalLibrary.Entities;
using MyJournalLibrary.Repositories.EntityRepositories;
using MyJournalLibrary.Services;

namespace MyJournal.ViewModels.UserControlsViewModels;

public class TeacherMarksViewModel : ViewModel
{
    private List<Subject> _subjects;
    private List<Class> _classes;
    
    public List<Subject> Subjects
    {
        get => _subjects;
        set => SetField(ref _subjects, value);
    }

    public List<Class> Classes
    {
        get => _classes;
        set => SetField(ref _classes, value);
    }
    public TeacherMarksViewModel()
    {
        using (var context = new ApplicationContext())
        {
            var employee = new EmployeesRepository(context).GetById(ApplicationData.UserId);
            Subjects = new SubjectsRepository(context).GetWithClassesByEmployee(employee).ToList();
        }
    }
}