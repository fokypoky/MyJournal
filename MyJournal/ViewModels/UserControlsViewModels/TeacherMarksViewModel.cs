using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MyJournal.Infrastructure.Commands;
using MyJournal.Models;
using MyJournal.ViewModels.Base;
using MyJournalLibrary.Entities;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournal.ViewModels.UserControlsViewModels;

public class TeacherMarksViewModel : ViewModel
{
    private List<Subject> _subjects;
    private Subject _expandedSubject;
    
    public ICommand OnSubjectClassTreeViewSelectedItemChanged
    {
        get => new RelayCommand((object parameter) =>
        {
            if (parameter is Subject)
            {
                MessageBox.Show("SUBJECT");
            }
            if (parameter is Class)
            {
                Class selectedClass = (Class)parameter;
                // остальная логика...
                MessageBox.Show("CLASS");
            }
        });
    }

    public ICommand OnSubjectClassTreeViewItemExpanded
    {
        get => new RelayCommand((object parameter) =>
        {
            MessageBox.Show("EXPANDED");
            /*if (parameter is Subject)
            {
                _expandedSubject = (Subject)parameter;
            }*/
        });
    }

    public List<Subject> Subjects
    {
        get => _subjects;
        set => SetField(ref _subjects, value);
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