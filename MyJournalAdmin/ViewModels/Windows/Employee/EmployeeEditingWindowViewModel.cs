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

namespace MyJournalAdmin.ViewModels.Windows.Employee
{
	public class EmployeeEditingWindowViewModel : ViewModel
	{
		private MyJournalLibrary.Entities.Employee _selectedEmployee;

		private ObservableCollection<Class> _classes;
		private ObservableCollection<Subject> _subjects;

		private Subject _selectedToDeleteSubject;
		private Class _selectedToDeleteClass;

		private INotifier _notifier;

		private string _newPassword;

		#region Public fields

		public Subject SelectedToDeleteSubject
		{
			get => _selectedToDeleteSubject;
			set => SetField(ref _selectedToDeleteSubject, value);
		}

		public Class SelectedToDeleteClass
		{
			get => _selectedToDeleteClass;
			set => SetField(ref _selectedToDeleteClass, value);
		}

		public MyJournalLibrary.Entities.Employee SelectedEmployee
		{
			get => _selectedEmployee;
			set => SetField(ref _selectedEmployee, value);
		}

		public string NewPassword
		{
			get => _newPassword;
			set => SetField(ref _newPassword, value);
		}

		#endregion

		#region Commands

		public ICommand ApplyContactChangesCommand
		{
			get => new RelayCommand(ApplyContactChanges);
		}

		public ICommand GeneratePasswordCommand
		{
			get => new RelayCommand(GeneratePassword);
		}

		public ICommand RemoveSubjectCommand
		{
			get => new RelayCommand(RemoveSubject);
		}

		#endregion

		#region Command functions

		private void RemoveSubject(object parameter)
		{
			if (SelectedEmployee is null)
			{
				_notifier.Notify("Нет данных о сотруднике");
				return;
			}

			if (SelectedToDeleteSubject is null)
			{
				_notifier.Notify("Предмет не выбран");
				return;
			}

			using (var context = new ApplicationContext())
			{
				new EmployeesRepository(context).RemoveSubject(SelectedEmployee, SelectedToDeleteSubject);
			}

			_notifier.Notify($"Предмет '{SelectedToDeleteSubject.SubjectTitle}' удален у сотрудника " +
			                 $"{SelectedEmployee.Contacts.Surname} {SelectedEmployee.Contacts.Name} " +
			                 $"{SelectedEmployee.Contacts.Midname}");

			Subjects.Remove(SelectedToDeleteSubject);
		}

		private void ApplyContactChanges(object parameter)
		{
			if (!IsContactsInputValid())
			{
				_notifier.Notify("Ошибка ввода. Поля не должны быть пустыми");
				return;
			}

			if (!String.IsNullOrEmpty(NewPassword))
			{
				// TODO: хэшировать пароль
				SelectedEmployee.Contacts.Password = NewPassword;
			}

			using (var context = new ApplicationContext())
			{
				var contactsRepository = new ContactsRepository(context);
				contactsRepository.Update(SelectedEmployee.Contacts);
				_notifier.Notify("Данные обновлены");
			}
		}
		
		private void GeneratePassword(object parameter) { }

		#endregion

		#region Public collections

		public ObservableCollection<Class> Classes
		{
			get => _classes;
			set => SetField(ref _classes, value);
		}

		public ObservableCollection<Subject> Subjects
		{
			get => _subjects;
			set => SetField(ref _subjects, value);
		}

		#endregion

		public EmployeeEditingWindowViewModel()
		{
			_notifier = new MessageBoxNotifier();
			WindowMessenger.MessageSender += OnMessageReceived;
		}

		private void OnMessageReceived(object? sender, EventArgs e)
		{
			if (e is EmployeeMessage)
			{
				var employeeMessage = (EmployeeMessage)e;
				SelectedEmployee = employeeMessage.Employee;
				
				LoadData();

				WindowMessenger.MessageSender -= OnMessageReceived;
			}
		}

		private void LoadData()
		{
			using (var context = new ApplicationContext())
			{
				Subjects = new ObservableCollection<Subject>(
					new SubjectsRepository(context).GetByEmployee(SelectedEmployee)
				);
			}
		}

		private bool IsContactsInputValid()
		{
			if (String.IsNullOrEmpty(SelectedEmployee.Contacts.Surname))
			{
				return false;
			}

			if (String.IsNullOrEmpty(SelectedEmployee.Contacts.Name))
			{
				return false;
			}

			if (String.IsNullOrEmpty(SelectedEmployee.Contacts.Midname))
			{
				return false;
			}

			if (String.IsNullOrEmpty(SelectedEmployee.Contacts.PhoneNumber))
			{
				return false;
			}

			if (String.IsNullOrEmpty(SelectedEmployee.Contacts.Email))
			{
				return false;
			}

			return true;
		}
	}
}
