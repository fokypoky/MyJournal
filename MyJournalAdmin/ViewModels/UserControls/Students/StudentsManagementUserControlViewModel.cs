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
using MyJournalAdmin.Views.Windows.Students;
using MyJournalLibrary.Entities;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournalAdmin.ViewModels.UserControls.Students
{
	public class StudentsManagementUserControlViewModel : ViewModel
	{
		private ObservableCollection<Student>? _students;
		private Student? _selectedStudent;
		private readonly INotifier _notifier;

		public ObservableCollection<Student>? Students
		{
			get => _students;
			set => SetField(ref _students, value);
		}

		public Student? SelectedStudent
		{
			get => _selectedStudent;
			set => SetField(ref _selectedStudent, value);
		}

		#region Commands

		public ICommand AddNewStudentCommand
		{
			get => new RelayCommand(AddNewStudent);
		}

		public ICommand UpdateStudentCommand
		{
			get => new RelayCommand(UpdateStudent);
		}

		public ICommand DeleteStudentCommand
		{
			get => new RelayCommand(DeleteStudent);
		}

		#endregion

		#region Command methods

		private void AddNewStudent(object parameter)
		{
			new AddNewStudentWindow().ShowDialog();
		}

		private void UpdateStudent(object parameter) { }

		private void DeleteStudent(object parameter)
		{
			if (SelectedStudent is null)
			{
				_notifier.Notify("Ученик не выбран");
				return;
			}
			
			using (var context = new ApplicationContext())
			{
				var marksRepository = new MarksRepository(context);
				var parentStudentRepository = new ParentStudentRepository(context);

				marksRepository.RemoveRange(marksRepository.GetByStudent(SelectedStudent));
				parentStudentRepository.RemoveRange(parentStudentRepository.GetByStudent(SelectedStudent));
				new StudentsRepository(context).Remove(SelectedStudent);
				new ContactsRepository(context).Remove(SelectedStudent.Contacts);

				Students?.Remove(SelectedStudent);
			}

			_notifier.Notify("Ученик удален");
			SelectedStudent = null;
		}

		#endregion

		private void OnMessageReceived(object? sender, EventArgs e)
		{
			if (e is NewStudentMessage)
			{
				var studentMessage = (NewStudentMessage)e;
				Students.Add(studentMessage.NewStudent);
				Students.OrderBy(s => s.Contacts?.Surname);
			}
		}
		
		public StudentsManagementUserControlViewModel()
		{
			WindowMessenger.MessageSender += OnMessageReceived;
			
			using (var context = new ApplicationContext())
			{
				Students = new ObservableCollection<Student>(
					new StudentsRepository(context).GetAllWithClassAndContacts()
						.OrderBy(s => s.Contacts?.Surname)
				);
			}

			_notifier = new MessageBoxNotifier();
		}
	}
}
