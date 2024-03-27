using System;
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

namespace MyJournalAdmin.ViewModels.Windows.Subjects
{
	public class AddNewSubjectWindowViewModel : ViewModel
	{
		private Subject _newSubject;
		private INotifier _notifier;

		public Subject NewSubject
		{
			get => _newSubject;
			set => SetField(ref _newSubject, value);
		}

		public ICommand AddSubjectCommand
		{
			get => new RelayCommand(AddSubject);
		}

		private void AddSubject(object parameter)
		{
			if (String.IsNullOrEmpty(NewSubject.SubjectTitle) || String.IsNullOrWhiteSpace(NewSubject.SubjectTitle))
			{
				_notifier.Notify("Некорректное название предмета");
				return;
			}

			using (var context = new ApplicationContext())
			{
				var subjectsRepository = new SubjectsRepository(context);
				if (subjectsRepository.IsSubjectTitleExists(NewSubject))
				{
					_notifier.Notify("Название предмета уже используется");
					return;
				}

				subjectsRepository.Add(NewSubject);
				_notifier.Notify("Предмет добавлен");
				WindowMessenger.OnMessageSend(new NewSubjectMessage() { Subject = NewSubject });
			}
		}

		public AddNewSubjectWindowViewModel()
		{
			NewSubject = new Subject();
			_notifier = new MessageBoxNotifier();
		}
	}
}
