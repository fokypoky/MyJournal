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
    private Subject _selectedSubject;
    
    public List<Subject> Subjects
    {
        get => _subjects;
        set => SetField(ref _subjects, value);
    }

    public ICommand OnTreeViewItemDoubleClick
    {
        get => new RelayCommand((object parameter) =>
        {
            if (parameter is Subject)
            {
                _selectedSubject = (Subject)parameter;
            }

            if (parameter is Class)
            {
                var selectedClass = (Class)parameter;

                if (_selectedSubject != null && selectedClass != null)
                {
                    MessageBox.Show($"SELECTED {_selectedSubject.SubjectTitle} {selectedClass.ClassNumber}");
                }
            }
        });
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