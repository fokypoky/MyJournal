using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MyJournal.Infrastructure.Commands;
using MyJournal.Models;
using MyJournal.ViewModels.Base;
using MyJournal.Views;
using MyJournalLibrary.Entities;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournal.ViewModels.UserControlsViewModels;

public class TeacherSelectionMarksUserControlViewModel : ViewModel
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
                    var window = new TeacherMarksWindow();
                    window.Show();
                    
                    WindowMessanger.OnMessageSend(new ClassSubjectMessage() {Class = selectedClass, Subject = _selectedSubject});
                }
            }
        });
    }
    public TeacherSelectionMarksUserControlViewModel()
    {
        using (var context = new ApplicationContext())
        {
            var employee = new EmployeesRepository(context).GetById(ApplicationData.UserId);
            Subjects = new SubjectsRepository(context).GetWithClassesByEmployee(employee).ToList();
        }
    }
}