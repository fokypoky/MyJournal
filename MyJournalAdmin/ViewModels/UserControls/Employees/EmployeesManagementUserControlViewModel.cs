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
using MyJournalAdmin.Views.Windows.Employee;
using MyJournalLibrary.Entities;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournalAdmin.ViewModels.UserControls.Employees
{
    public class EmployeesManagementUserControlViewModel : ViewModel
    {
		private ObservableCollection<Employee> _employees;

		private string _employeeSurnameToSearch;
		private string _employeeNameToSearch;
		private string _employeePhoneNumberToSearch;
		private string _employeeEmailToSearch;

		private Employee _selectedEmployee;

		private INotifier _notifier;

		#region Public fields

		public INotifier Notifier { get; set; }

		public Employee SelectedEmployee
		{
			get => _selectedEmployee;
			set => SetField(ref _selectedEmployee, value);
		}

		public string EmployeeSurnameToSearch
		{
			get => _employeeSurnameToSearch;
			set => SetField(ref _employeeSurnameToSearch, value);
		}

		public string EmployeeNameToSearch
		{
			get => _employeeNameToSearch; 
			set=> SetField(ref _employeeNameToSearch,value);
		}

		public string EmployeePhoneNumberToSearch
		{
			get => _employeePhoneNumberToSearch;
			set => SetField(ref _employeePhoneNumberToSearch, value);
		}

		public string EmployeeEmailToSearch
		{
			get => _employeeEmailToSearch;
			set => SetField(ref _employeeEmailToSearch, value);
		}

		#endregion

		#region Public collections

		public ObservableCollection<Employee> Employees
	    {
			get => _employees;
			set => SetField(ref _employees, value);
	    }

		#endregion

		#region Commands

		public ICommand FindEmployeeCommand
		{
			get => new RelayCommand(FindEmployee);
		}

		public ICommand AddEmployeeCommand
		{
			get => new RelayCommand(AddEmployee);
		}

		public ICommand ChangeEmployeeInfoCommand
		{
			get => new RelayCommand(ChangeEmployeeInfo);
		}

		public ICommand RemoveEmployeeCommand
		{
			get => new RelayCommand(RemoveEmployee);
		}

		#endregion

		#region Command functions

		private void FindEmployee(object parameter) { }

		private void AddEmployee(object parameter)
		{
			var addingWindow = new AddNewEmployeeWindow();
			addingWindow.ShowDialog();
		}

		private void ChangeEmployeeInfo(object parameter)
		{
			if (SelectedEmployee is null)
			{
				Notifier.Notify("Сотрудник не выбран");
				return;
			}

			var editingWindow = new EmployeeEditingWindow();
			editingWindow.Show();
			
			WindowMessenger.OnMessageSend(new EmployeeMessage(SelectedEmployee));
		}

		private void RemoveEmployee(object parameter)
		{
			if (SelectedEmployee is null)
			{
				_notifier.Notify("Сотрудник не выбран");
				return;
			}

			using (var context = new ApplicationContext())
			{
				var marksRepository = new MarksRepository(context);
				var tasksRepository = new TasksRepository(context);
				var timetableRepository = new TimetableRepository(context);
				var classesRepository = new ClassRepository(context);

				new EmployeeSubjectRepository(context).RemoveByEmployee(SelectedEmployee);

				var employeeMarks = marksRepository.GetByEmployee(SelectedEmployee);
				foreach (var employeeMark in employeeMarks)
				{
					employeeMark.TeacherId = null;
				}

				marksRepository.UpdateRange(employeeMarks);

				var employeeTasks = tasksRepository.GetByEmployee(SelectedEmployee);
				foreach (var task in employeeTasks)
				{
					task.TeacherId = null;
				}

				tasksRepository.UpdateRange(employeeTasks.ToList());

				var classes = new ClassRepository(context).GetByEmployee(SelectedEmployee);
				foreach (var @class in classes)
				{
					@class.LeaderId = null;
				}

				classesRepository.UpdateRange(classes.ToList());

				var timetables = timetableRepository.GetByEmployee(SelectedEmployee);
				foreach (var timetable in timetables)
				{
					timetable.TeacherId = null;
				}

				timetableRepository.UpdateRange(timetables.ToList());

				new EmployeesRepository(context).Remove(SelectedEmployee);
				new ContactsRepository(context).Remove(SelectedEmployee.Contacts);

				Employees.Remove(SelectedEmployee);
				SelectedEmployee = null;

				_notifier.Notify("Сотрудник удален");
			}

		}

		#endregion

		public EmployeesManagementUserControlViewModel()
		{
			WindowMessenger.MessageSender += OnMessageReceived;

			_notifier = new MessageBoxNotifier();

		    using (var context = new ApplicationContext())
		    {
			    Employees = new ObservableCollection<Employee>(
				    new EmployeesRepository(context).GetAllWithContacts()
			    );
		    }

		    Notifier = new MessageBoxNotifier();

	    }

		/// <summary>
		/// Отображение добавленных пользователей в AddNewEmployeeWindow
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnMessageReceived(object? sender, EventArgs e)
		{
			if (e is NewEmployeeMessage)
			{
				var employeeMessage = (NewEmployeeMessage)e;
				Employees.Add(employeeMessage.Employee);
			}
		}

    }
}
