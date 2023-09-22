using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MyJournalAdmin.Infrastructure.Commands;
using MyJournalAdmin.Infrastructure.Repositories;
using MyJournalAdmin.Models;
using MyJournalAdmin.ViewModels.Base;
using MyJournalAdmin.Views.Notifiers.Implementation;
using MyJournalAdmin.Views.Notifiers.Interfaces;
using MyJournalAdmin.Views.Windows.Main;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournalAdmin.ViewModels.Windows.Login
{
	public class LoginWindowViewModel : ViewModel
	{
		private string _login;
		private string _password;
		private bool _needsToSaveLogin;

		#region Public fields

		public INotifier Notifier;

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
			get => new RelayCommand(LogIn, CanConectToDb);
		}

		public ICommand OpenConnectionSettingsCommand
		{
			get => new RelayCommand(OpenConnectionSettings);
		}

		#endregion

		#region Command functions

		private void RememberLogin(object parameter) => NeedsToSaveLogin = !NeedsToSaveLogin;

		private void LogIn(object parameter)
		{
			using (var context = new ApplicationContext())
			{
				var contactsRepository = new ContactsRepository(context);
				var contact = contactsRepository.GetByLogin(Login, Password);

				if (contact == null)
				{
					Notifier.Notify("Такого пользователя не существует");
					return;
				}

				ApplicationData.UserId = contact.Id;

				if (contact.UserRole.Rolename != "admin")
				{
					Notifier.Notify("Недостаточно привилегий");
					return;
				}

				var mainWindow = new MainWindow();
				mainWindow.Show();

				if (parameter is Window)
				{
					var currentWindow = (Window) parameter;
					currentWindow.Close();
				}
			}
		}

		private bool CanConectToDb(object parameter)
		{
			using (var context = new ApplicationContext())
			{
				try
				{
					return context.Database.CanConnect();
				}
				catch
				{
					return false;
				}
			}
		}

		private void OpenConnectionSettings(object parameter) { }


		#endregion

		public LoginWindowViewModel()
		{
			Notifier = new MessageBoxNotifier();
		}

	}
}
