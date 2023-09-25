using System;
using System.Windows.Input;
using MyJournalAdmin.Infrastructure.Commands;
using MyJournalAdmin.Models.Messenging;
using MyJournalAdmin.Models.Messenging.MessageTypes;
using MyJournalAdmin.ViewModels.Base;

namespace MyJournalAdmin.ViewModels.Windows.Employee
{
	public class EmployeeEditingWindowViewModel : ViewModel
	{
		private MyJournalLibrary.Entities.Employee _selectedEmployee;
		
		#region Public fields

		public MyJournalLibrary.Entities.Employee SelectedEmployee
		{
			get => _selectedEmployee;
			set => SetField(ref _selectedEmployee, value);
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

		#endregion

		#region Command functions

		private void ApplyContactChanges(object parameter) { }
		
		private void GeneratePassword(object parameter) { }

		#endregion

		public EmployeeEditingWindowViewModel()
		{
			WindowMessenger.MessageSender += OnMessageReceived;
		}

		private void OnMessageReceived(object? sender, EventArgs e)
		{
			if (e is EmployeeMessage)
			{
				var employeeMessage = (EmployeeMessage)e;
				SelectedEmployee = employeeMessage.Employee;
				
				WindowMessenger.MessageSender -= OnMessageReceived;
			}
		}
	}
}
