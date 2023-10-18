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
    public class SubjectUpdatingWindowViewModel : ViewModel
    {
	    private Subject _subject;
		private INotifier _notifier;
		private string _newSubjectTitle;

	    public Subject Subject
	    {
			get => _subject;
			set => SetField(ref _subject, value);
	    }

	    public string NewSubjectTitle
	    {
		    get => _newSubjectTitle;
		    set => SetField(ref _newSubjectTitle, value);
	    }

	    public ICommand UpdateSubjectCommand
	    {
		    get => new RelayCommand(UpdateSubject);
	    }

	    private void UpdateSubject(object parameter)
	    {
		    if (String.IsNullOrEmpty(NewSubjectTitle) || String.IsNullOrWhiteSpace(NewSubjectTitle))
		    {
				_notifier.Notify("Некорректное название предмета");
				return;
		    }

		    if (NewSubjectTitle == Subject.SubjectTitle)
		    {
			    return;
		    }

		    using (var context = new ApplicationContext())
		    {
			    var subjectsRepository = new SubjectsRepository(context);
			    if (subjectsRepository.IsSubjectTitleExists(new Subject() { SubjectTitle = NewSubjectTitle }))
			    {
					_notifier.Notify("Название предмета уже используется");
					return;
			    }

				Subject.SubjectTitle = NewSubjectTitle;
				subjectsRepository.Update(Subject);
				_notifier.Notify("Предмет успешно изменен");

				WindowMessenger.OnMessageSend(new UpdatedSubjectMessage() { Subject = Subject });
		    }

	    }

	    public SubjectUpdatingWindowViewModel()
	    {
		    WindowMessenger.MessageSender += OnMessageReceived;
		    _notifier = new MessageBoxNotifier();
	    }

	    private void OnMessageReceived(object? sender, EventArgs e)
	    {
		    if (e is SubjectToUpdateMessage)
		    {
				var message = (SubjectToUpdateMessage)e;
				Subject = message.Subject;
				NewSubjectTitle = Subject.SubjectTitle;
				WindowMessenger.MessageSender -= OnMessageReceived;
		    }
	    }
    }
}
