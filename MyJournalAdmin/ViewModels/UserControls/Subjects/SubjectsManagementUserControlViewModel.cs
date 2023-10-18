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
		private void RemoveSubject(object parameter) { }
		private void UpdateSubject(object parameter) { }

		#endregion

		private void OnMessageReceived(object? sender, EventArgs e)
		{
			if (e is NewSubjectMessage)
			{
				var message = (NewSubjectMessage)e;
				AllSubjects.Add(message.Subject);
				AllSubjects.OrderBy(s => s.SubjectTitle);
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
