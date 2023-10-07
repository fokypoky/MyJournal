using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MyJournalAdmin.Infrastructure.Commands;
using MyJournalAdmin.Infrastructure.Repositories;
using MyJournalAdmin.Models.Messenging;
using MyJournalAdmin.Models.Messenging.MessageTypes;
using MyJournalAdmin.ViewModels.Base;
using MyJournalAdmin.Views.Notifiers.Implementation;
using MyJournalAdmin.Views.Notifiers.Interfaces;
using MyJournalLibrary.Entities;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournalAdmin.ViewModels.Windows.Auditories
{
    public class AuditoryUpdatingWindowViewModel : ViewModel
    {
	    private Auditory _auditory;

	    private string _newAuditoryNumber;

		private INotifier _notifier;

		private ObservableCollection<Class> _allClasses;
		private Class? _selectedClass;

	    #region Public fields

	    public Auditory Auditory
	    {
		    get => _auditory;
		    set => SetField(ref _auditory, value);
	    }

	    public string NewAuditoryNumber
	    {
		    get => _newAuditoryNumber;
		    set => SetField(ref _newAuditoryNumber, value);
	    }

	    public Class? SelectedClass
	    {
		    get => _selectedClass;
		    set => SetField(ref _selectedClass, value);
	    }

		#endregion

		#region Public collections

		public ObservableCollection<Class> AllClasses
		{
			get => _allClasses;
			set => SetField(ref _allClasses, value);
		}

		#endregion

		#region Commands

		public ICommand SaveChangesCommand
		{
			get => new RelayCommand(SaveChanges);
		}

		#endregion

		#region Command function

		private void SaveChanges(object parameter)
		{
			if (String.IsNullOrEmpty(NewAuditoryNumber))
			{
				_notifier.Notify("Номер аудитории не может быть пустым");
				return;
			}

			if (NewAuditoryNumber.Length > 10)
			{
				_notifier.Notify("Номер аудитории не может быть длиннее 10 символов");
				return;
			}

			using (var context = new ApplicationContext())
			{
				var auditoriesRepository = new AuditoriesRepository(context);
				var classRepository = new ClassRepository(context);

				if (NewAuditoryNumber != Auditory.AuditoryNumber &&
				    auditoriesRepository.IsAuditoryNumberExists(NewAuditoryNumber))
				{
					_notifier.Notify("Номер аудитории занят");
					return;
				}

				Auditory.AuditoryNumber = NewAuditoryNumber;
				auditoriesRepository.Update(Auditory);

				// если класс поменялся
				if (SelectedClass is not null)
				{
					var previousClass = classRepository.GetByAuditory(Auditory);
					if (SelectedClass?.ClassNumber != previousClass?.ClassNumber)
					{
						previousClass.AuditoryId = null;
						SelectedClass.AuditoryId = Auditory.Id;

						classRepository.UpdateRange(new[] { previousClass, SelectedClass });
					}
				}

			}

		} 

		#endregion

		private void OnMessageReceived(object? sender, EventArgs e)
	    {
		    if (e is AuditoryEditMessage)
		    {
				var editMessage = (AuditoryEditMessage)e;
				Auditory = editMessage.Auditory;

				LoadData();

				WindowMessenger.MessageSender -= OnMessageReceived;
		    }
	    }

		private void LoadData()
		{
			NewAuditoryNumber = Auditory.AuditoryNumber;

			using (var context = new ApplicationContext())
			{
				var classRepository = new ClassRepository(context);

				AllClasses = new ObservableCollection<Class>(
					classRepository.GetAll()
				);
				SelectedClass = classRepository.GetByAuditory(Auditory);
			}

		}

	    public AuditoryUpdatingWindowViewModel()
	    {
		    _notifier = new MessageBoxNotifier();

		    WindowMessenger.MessageSender += OnMessageReceived;
	    }
    }
}
