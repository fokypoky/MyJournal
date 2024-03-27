using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyJournal.Models;
using MyJournal.ViewModels.Base;
using MyJournalLibrary.Entities;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournal.ViewModels.UserControlsViewModels.Student;

public class StudentProfileUserControlViewModel : ViewModel
{
    private MyJournalLibrary.Entities.Student _student;
    private ObservableCollection<Subject> _subjects;

    public MyJournalLibrary.Entities.Student Student
    {
        get => _student;
        set => SetField(ref _student, value);
    }

    #region Collections

    public ObservableCollection<Subject> Subjects
    {
        get => _subjects;
        set => SetField(ref _subjects, value);
    }

    #endregion

    public StudentProfileUserControlViewModel()
    {
        using (var context = new ApplicationContext())
        {
            Student = new StudentsRepository(context).GetByIdWithContactsClassAndSubjects(ApplicationData.UserId);
            Subjects = new ObservableCollection<Subject>(
                new SubjectsRepository(context).GetByClass(Student.Class)
            );
        }
    }
}