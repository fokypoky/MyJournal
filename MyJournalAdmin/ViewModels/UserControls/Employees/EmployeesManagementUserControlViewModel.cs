using System.Collections.ObjectModel;
using System.Windows.Input;
using MyJournalAdmin.Infrastructure.Commands;
using MyJournalAdmin.Infrastructure.Repositories;
using MyJournalAdmin.ViewModels.Base;
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

		#region Public fields

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

		private void AddEmployee(object parameter) { }

		private void ChangeEmployeeInfo(object parameter) { }

		private void RemoveEmployee(object parameter) { }

		#endregion

		public EmployeesManagementUserControlViewModel()
	    {
		    using (var context = new ApplicationContext())
		    {
			    Employees = new ObservableCollection<Employee>(
				    new EmployeesRepository(context).GetAllWithContacts()
			    );
		    }
	    }

    }
}
