using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyJournal.Models;
using MyJournal.ViewModels.Base;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournal.ViewModels.UserControlsViewModels.Parent;

public class ParentTimetableUserControlViewModel : ViewModel
{
    private MyJournalLibrary.Entities.Parent _parent;
    private ObservableCollection<MyJournalLibrary.Entities.Student> _students;
    private MyJournalLibrary.Entities.Student _selectedStudent;

    public MyJournalLibrary.Entities.Student SelectedStudent
    {
        get => _selectedStudent;
        set => SetField(ref _selectedStudent, value);
    }
    #region Collections

    public ObservableCollection<MyJournalLibrary.Entities.Student> Students
    {
        get => _students;
        set => SetField(ref _students, value);
    }

    #endregion

    public ParentTimetableUserControlViewModel()
    {
        using (var context = new ApplicationContext())
        {
	        _parent = new ParentsRepository(context).GetByUserId(ApplicationData.UserId);

            Students = new ObservableCollection<MyJournalLibrary.Entities.Student>(
                new StudentsRepository(context).GetWithContactsByParent(_parent)
            );
        }
    }
}