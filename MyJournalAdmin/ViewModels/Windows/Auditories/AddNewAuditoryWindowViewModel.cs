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
	public class AddNewAuditoryWindowViewModel : ViewModel
	{
		private Auditory _auditory;

		private ObservableCollection<Class> _classes;
		private Class _selectedClass;
		
		private bool _classIsNotNeed = false;

		private INotifier _notifier;

		#region Public fields

		public Auditory Auditory
		{
			get => _auditory;
			set => SetField(ref _auditory, value);
		}

		/// <summary>
		/// Нужно ли добавлять аудитории класс
		/// </summary>
		public bool ClassIsNotNeed
		{
			get => _classIsNotNeed;
			set
			{
				SetField(ref _classIsNotNeed, value);
				OnPropertyChanged(nameof(ClassSelectionIsAvailable));
			}
		}

		public Class SelectedClass
		{
			get => _selectedClass;
			set => SetField(ref _selectedClass, value);
		}

		public bool ClassSelectionIsAvailable
		{
			get => !ClassIsNotNeed;
		}

		#endregion

		#region Public collections

		public ObservableCollection<Class> Classes
		{
			get => _classes;
			set => SetField(ref _classes, value);
		}

		#endregion

		#region Commands

		public ICommand AddAuditoryCommand
		{
			get => new RelayCommand(AddAuditory);
		}

		#endregion

		#region Command functions

		private void AddAuditory(object parameter)
		{
			if (String.IsNullOrEmpty(Auditory.AuditoryNumber))
			{
				_notifier.Notify("Номер аудитории не может быть пустым");
				return;
			}

			if (Auditory.AuditoryNumber.Length > 10)
			{
				_notifier.Notify("Номер аудитории не может быть больше 10 символов");
				return;
			}

			// если класс нужен, но не выбран
			if (SelectedClass is null && !ClassIsNotNeed)
			{
				_notifier.Notify("Класс не выбран");
				return;
			}

			using (var context = new ApplicationContext())
			{
				var auditoriesRepository = new AuditoriesRepository(context);

				if (auditoriesRepository.IsAuditoryNumberExists(Auditory.AuditoryNumber))
				{
					_notifier.Notify("Аудитория с таким номером уже существует");
					return;
				}

				auditoriesRepository.Add(Auditory);

				WindowMessenger.OnMessageSend(new NewAuditoryMessage() { Auditory = Auditory });
				_notifier.Notify("Аудитория добавлена");

				if (ClassIsNotNeed)
				{
					return;
				}

				SelectedClass.AuditoryId = Auditory.Id;
				new ClassRepository(context).Update(SelectedClass);
			}
		}

		#endregion

		public AddNewAuditoryWindowViewModel()
		{
			Auditory = new Auditory();

			_notifier = new MessageBoxNotifier();

			using (var context = new ApplicationContext())
			{
				Classes = new ObservableCollection<Class>(
					new ClassRepository(context).GetAll()
				);
			}
		}
	}
}
