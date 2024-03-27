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

namespace MyJournalAdmin.ViewModels.Windows.Classes
{
    public class UpdateClassWindowViewModel : ViewModel
    {
	    private INotifier _notifier;
	    private Class _class;

	    private Auditory _selectedAuditory;
	    private MyJournalLibrary.Entities.Employee _selectedLeader;

	    private ObservableCollection<Auditory> _availableAuditories;
	    private ObservableCollection<MyJournalLibrary.Entities.Employee> _availableEmployees;

	    private bool _isAuditoryNotRequired;
	    private bool _isLeaderNotRequired;

	    private bool _isAuditoriesSwitchingEnabled;
	    private bool _isLeaderSwitchingEnabled;

	    private string _previousClassNumber;

	    public bool IsAuditoryNotRequired
	    {
		    get => _isAuditoryNotRequired;
		    set
		    {
			    SetField(ref _isAuditoryNotRequired, value);
				OnPropertyChanged(nameof(IsAuditoriesSwitchingEnabled));
		    }
	    }

	    public bool IsLeaderNotRequired
	    {
		    get => _isLeaderNotRequired;
		    set
		    {
			    SetField(ref _isLeaderNotRequired, value);
				OnPropertyChanged(nameof(IsLeaderSwitchingEnabled));
		    }
	    }

	    public bool IsAuditoriesSwitchingEnabled
	    {
		    get => !IsAuditoryNotRequired;
	    }

	    public bool IsLeaderSwitchingEnabled
	    {
		    get => !IsLeaderNotRequired;
	    }

		public Auditory SelectedAuditory
	    {
		    get => _selectedAuditory;
		    set => SetField(ref _selectedAuditory, value);
	    }

	    public MyJournalLibrary.Entities.Employee SelectedLeader
	    {
		    get => _selectedLeader;
		    set => SetField(ref _selectedLeader, value);
	    }

	    public ObservableCollection<Auditory> AvailableAuditories
	    {
		    get => _availableAuditories;
		    set => SetField(ref _availableAuditories, value);
	    }

	    public ObservableCollection<MyJournalLibrary.Entities.Employee> AvailableEmployees
	    {
		    get => _availableEmployees;
		    set => SetField(ref _availableEmployees, value);
	    }

	    public Class Class
	    {
		    get => _class;
		    set => SetField(ref _class, value);
	    }

	    private void OnMessageReceived(object? sender, EventArgs e)
	    {
		    if (e is ClassToUpdateMessage)
		    {
				var classMessage = (ClassToUpdateMessage)e;

				Class = classMessage.Class;

			    using (var context = new ApplicationContext())
			    {
				    AvailableAuditories = new(
					    new AuditoriesRepository(context).GetFreeAuditories()
				    );
				    AvailableEmployees = new(
					    new EmployeesRepository(context).GetFreeClassEmployeesWithContacts()
				    );
			    }

			    _previousClassNumber = classMessage.Class.ClassNumber;
			    WindowMessenger.MessageSender -= OnMessageReceived;
		    }
	    }

	    public ICommand UpdateClassCommand
	    {
		    get => new RelayCommand(UpdateClass);
	    }

	    private void UpdateClass(object parameter)
	    {
		    if (String.IsNullOrEmpty(Class.ClassNumber))
		    {
			    _notifier.Notify("Укажите номер класса");
			    return;
		    }

		    if (!IsAuditoryNotRequired && SelectedAuditory is null)
		    {
			    _notifier.Notify("Укажите аудиторию");
			    return;
		    }

		    if (!IsLeaderNotRequired && SelectedLeader is null)
		    {
			    _notifier.Notify("Укажите руководителя");
			    return;
		    }

		    using (var context = new ApplicationContext())
		    {
			    var classRepository = new ClassRepository(context);
			    if (classRepository.IsClassNumberExists(Class.ClassNumber) && Class.ClassNumber != _previousClassNumber)
			    {
					_notifier.Notify("Класс с таким номером уже существует");
					return;
			    }

			    var @class = new Class() { Id = Class.Id };
			    
			    if (SelectedAuditory is not null)
			    {
					@class.AuditoryId = SelectedAuditory.Id;
			    }

			    if (SelectedLeader is not null)
			    {
					@class.LeaderId = SelectedLeader.Id;
			    }

				classRepository.UpdateNoTracked(Class, @class);
				_notifier.Notify("Класс обновлен");
		    }
		}

	    public UpdateClassWindowViewModel()
	    {
		    _notifier = new MessageBoxNotifier();
		    WindowMessenger.MessageSender += OnMessageReceived;
		    Class = new Class();
	    }
    }
}
