using System;
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
