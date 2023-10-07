using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MyJournalAdmin.Infrastructure.Commands;
using MyJournalAdmin.Infrastructure.Repositories;
using MyJournalAdmin.Models.Messenging;
using MyJournalAdmin.Models.Messenging.MessageTypes;
using MyJournalAdmin.ViewModels.Base;
using MyJournalAdmin.Views.Notifiers.Implementation;
using MyJournalAdmin.Views.Notifiers.Interfaces;
using MyJournalAdmin.Views.Windows.Auditories;
using MyJournalLibrary.Entities;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournalAdmin.ViewModels.UserControls.Auditories
{
    public class AuditoriesManagementUserControlViewModel : ViewModel
    {
	    private ObservableCollection<Auditory> _auditories;
	    private Auditory _selectedAuditory;

		private INotifier _notifier;

		#region Public fields

	    public Auditory SelectedAuditory
	    {
		    get => _selectedAuditory;
		    set => SetField(ref _selectedAuditory, value);
	    }

	    #endregion

		#region Public collections

		public ObservableCollection<Auditory> Auditories
	    {
		    get => _auditories;
		    set => SetField(ref _auditories, value);
	    }

		#endregion

		#region Commands

		public ICommand AddAuditoryCommand
		{
			get => new RelayCommand(AddAuditory);
		}

		public ICommand RemoveAuditoryCommand
		{
			get => new RelayCommand(RemoveAuditory);
		}

		public ICommand UpdateAuditoryCommand
		{
			get => new RelayCommand(UpdateAuditory);
		}

		#endregion

		#region Command functions

		private void AddAuditory(object parameter)
		{
			new AddNewAuditoryWindow().Show();
		}
		private void RemoveAuditory(object parameter) { }

		private void UpdateAuditory(object parameter)
		{
			if (SelectedAuditory is null)
			{
				_notifier.Notify("Аудитория не выбрана");
				return;
			}

			new AuditoryUpdatingWindow().Show();
			WindowMessenger.OnMessageSend(new AuditoryEditMessage() {Auditory = SelectedAuditory});
		}

		#endregion

		private void OnMessageReceived(object? sender, EventArgs e)
		{
			if (e is NewAuditoryMessage)
			{
				var message = (NewAuditoryMessage)e;
				Auditories.Add(message.Auditory);
				Auditories.OrderBy(a => a.AuditoryNumber);
			}
		}

		public AuditoriesManagementUserControlViewModel()
		{
			_notifier = new MessageBoxNotifier();

			WindowMessenger.MessageSender += OnMessageReceived;

		    using (var context = new ApplicationContext())
		    {
				Auditories = new ObservableCollection<Auditory>(
					new AuditoriesRepository(context).GetAll()
						.OrderBy(a => a.AuditoryNumber)
				);
		    }
	    }
    }
}
