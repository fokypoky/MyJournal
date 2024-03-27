using MyJournal.Models;
using MyJournal.ViewModels.Base;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournal.ViewModels.UserControlsViewModels.Parent;

public class ParentProfileUserControlViewModel : ViewModel
{
	private MyJournalLibrary.Entities.Parent _parent;

	public MyJournalLibrary.Entities.Parent Parent
	{
		get => _parent;
		set => SetField(ref _parent, value);
	}

	public ParentProfileUserControlViewModel()
	{
		using (var context = new ApplicationContext())
		{
			Parent =
				new ParentsRepository(context)
					.GetByUserIdWithContactsStudentsAndStudentContacts(ApplicationData.UserId);
		}
	}
}