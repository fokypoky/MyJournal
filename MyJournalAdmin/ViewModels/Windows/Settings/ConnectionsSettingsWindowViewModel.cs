using System.Windows;
using System.Windows.Input;
using MyJournalAdmin.Infrastructure.Commands;
using MyJournalAdmin.Infrastructure.Repositories;
using MyJournalAdmin.Models;
using MyJournalAdmin.ViewModels.Base;
using MyJournalAdmin.Views.Notifiers.Implementation;
using MyJournalAdmin.Views.Notifiers.Interfaces;
using MyJournalLibrary.Repositories.FileRepositories;

namespace MyJournalAdmin.ViewModels.Windows.Settings
{
    public class ConnectionsSettingsWindowViewModel : ViewModel
    {
	    private string _host;
	    private string _port;
	    private string _database;
	    private string _username;
	    private string _password;

		#region Public fields

		public INotifier Notifier { get; set; }

		public string Host
		{
			get => _host;
			set => SetField(ref _host, value);
		}

		public string Port
		{
			get => _port;
			set => SetField(ref _port, value);
		}

		public string Database
		{
			get => _database;
			set => SetField(ref _database, value);
		}

		public string Username
		{
			get => _username;
			set => SetField(ref _username, value);
		}
		public string Password
		{
			get => _password;
			set => SetField(ref _password, value);
		}

		#endregion

		#region Commands

		public ICommand ApplySettingsCommand
		{
			get => new RelayCommand(ApplySettings);
		}

		#endregion

		#region Command functions

		private void ApplySettings(object parameter)
		{
			DatabaseConnection connection = new DatabaseConnection(Host, Port, Username, Password, Database);
			JsonRepository<DatabaseConnection> repository = new JsonRepository<DatabaseConnection>("ConnectionSettings.json");

			// TODO: довести до ума, метод не должен принимать параметр
			repository.WriteFile(connection);
			
			Notifier.Notify("Изменения применены. Перезапустите приложение");

			if (parameter is Window)
			{
				var currentWindow = (Window)parameter;
				currentWindow.Close();
			}

		} 

		#endregion

		public ConnectionsSettingsWindowViewModel()
		{
			Notifier = new MessageBoxNotifier();

			DatabaseConnection connection = new DatabaseConnection(ApplicationContext.ConnectionString);

			Host = connection.Host;
			Port = connection.Port;
			Username = connection.Username;
			Password = connection.Password;
			Database = connection.Database;
		}
	}
}
