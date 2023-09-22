using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MyJournalAdmin.Infrastructure.Commands;
using MyJournalAdmin.ViewModels.Base;

namespace MyJournalAdmin.ViewModels.Windows.Login
{
	public class LoginWindowViewModel : ViewModel
	{
		private string _login;
		private string _password;
		private bool _needsToSaveLogin;

		#region Public fields

		public string Login
		{
			get => _login;
			set => SetField(ref _login, value);
		}

		public string Password
		{
			get => _password;
			set => SetField(ref _password, value);
		}

		public bool NeedsToSaveLogin
		{
			get => _needsToSaveLogin;
			set => SetField(ref _needsToSaveLogin, value);
		}

		#endregion

		#region Commands

		public ICommand RememberLoginCommand
		{
			get => new RelayCommand(RememberLogin);
		}

		public ICommand LoginCommand
		{
			get => new RelayCommand(LogIn);
		}

		public ICommand OpenConnectionSettingsCommand
		{
			get => new RelayCommand(OpenConnectionSettings);
		}

		#endregion

		#region Command functions

		private void RememberLogin(object parameter) { }
		private void LogIn(object parameter) { }
		private void OpenConnectionSettings(object parameter) { }

		#endregion

		public LoginWindowViewModel()
		{
			
		}

	}
}
