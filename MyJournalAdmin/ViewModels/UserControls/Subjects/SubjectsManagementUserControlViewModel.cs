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
using MyJournalAdmin.Views.Windows.Subjects;
using MyJournalLibrary.Entities;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournalAdmin.ViewModels.UserControls.Subjects
{
    public class SubjectsManagementUserControlViewModel : ViewModel
    {
	    private ObservableCollection<Subject> _allSubjects;
	    private Subject _selectedSubject;

		private INotifier _notifier;

	    #region Public fields

	    public Subject SelectedSubject
	    {
		    get => _selectedSubject;
		    set => SetField(ref _selectedSubject, value);
	    }

	    #endregion

		#region Public collections

		public ObservableCollection<Subject> AllSubjects
	    {
		    get => _allSubjects;
		    set => SetField(ref _allSubjects, value);
	    }

		#endregion

		#region Commands

		public ICommand RefreshCommand
		{
			get => new RelayCommand(Refresh);
		}

		public ICommand AddSubjectCommand
		{
			get => new RelayCommand(AddSubject);
		}

		public ICommand RemoveSubjectCommand
		{
			get => new RelayCommand(RemoveSubject);
		}

		public ICommand UpdateSubjectCommand
		{
			get => new RelayCommand(UpdateSubject);
		}

		#endregion

		#region Command functions

		private void AddSubject(object parameter)
		{
			new AddNewSubjectWindow().Show();
		}

		private void RemoveSubject(object parameter)
		{
			if (SelectedSubject is null)
			{
				_notifier.Notify("Предмет не выбран");
				return;
			}

			using (var context = new ApplicationContext())
			{
				new ClassSubjectRepository(context).RemoveAllBySubject(SelectedSubject);
				new TasksRepository(context).RemoveAllBySubject(SelectedSubject);
				new TimetableRepository(context).RemoveAllBySubject(SelectedSubject);
				new EmployeeSubjectRepository(context).RemoveAllBySubject(SelectedSubject);
				new MarksRepository(context).RemoveAllBySubject(SelectedSubject);
				new SubjectsRepository(context).Remove(SelectedSubject);

				AllSubjects.Remove(SelectedSubject);
				SelectedSubject = null;

				_notifier.Notify("Предмет удален");
			}
		}

		private void UpdateSubject(object parameter)
		{
			if (SelectedSubject is null)
			{
				_notifier.Notify("Предмет не выбран");
				return;
			}

			new SubjectUpdatingWindow().Show();
			WindowMessenger.OnMessageSend(new SubjectToUpdateMessage() { Subject = SelectedSubject });
		}

		private void Refresh(object parameter)
		{
			using (var context = new ApplicationContext())
			{
				AllSubjects = new ObservableCollection<Subject>(
					new SubjectsRepository(context).GetAll()
				);
				AllSubjects.OrderBy(s => s.SubjectTitle);
			}

			SelectedSubject = null;
		}

		#endregion

		private void OnMessageReceived(object? sender, EventArgs e)
		{
			if (e is NewSubjectMessage)
			{
				var message = (NewSubjectMessage)e;
				AllSubjects.Add(message.Subject);
				AllSubjects.OrderBy(s => s.SubjectTitle);
			}

			if (e is UpdatedSubjectMessage)
			{
				var message = (UpdatedSubjectMessage)e;
				int subjectIndex = AllSubjects.IndexOf(AllSubjects.FirstOrDefault(s => s.Id == message.Subject.Id));
				AllSubjects.RemoveAt(subjectIndex);
				AllSubjects.Add(message.Subject);
				AllSubjects.OrderBy(s => s.SubjectTitle);
				OnPropertyChanged(nameof(AllSubjects));
			}
		}

		public SubjectsManagementUserControlViewModel()
		{
			WindowMessenger.MessageSender += OnMessageReceived;

			_notifier = new MessageBoxNotifier();

		    using (var context = new ApplicationContext())
		    {
			    AllSubjects = new ObservableCollection<Subject>(
				    new SubjectsRepository(context).GetAll()
				);
			    AllSubjects.OrderBy(s => s.SubjectTitle);
		    }
	    }
    }
}
