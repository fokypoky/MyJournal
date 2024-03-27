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
    public class AddNewClassWindowViewModel : ViewModel
    {
	    private INotifier _notifier;

	    private Class _class;
	    private ObservableCollection<Auditory> _availableAuditories;
	    private ObservableCollection<MyJournalLibrary.Entities.Employee> _availableEmployees;

	    private Auditory _selectedAuditory;
	    private MyJournalLibrary.Entities.Employee _selectedLeader;

	    private bool _isAuditoryNotRequired;
	    private bool _isLeaderNotRequired;

	    public MyJournalLibrary.Entities.Employee SelectedLeader
	    {
		    get => _selectedLeader;
		    set => SetField(ref _selectedLeader, value);
	    }

	    public Auditory SelectedAuditory
	    {
		    get => _selectedAuditory;
		    set => SetField(ref _selectedAuditory, value);
	    }

		public bool IsAuditoriesSwitchingEnabled
	    {
		    get => !IsAuditoryNotRequired;
	    }

	    public bool IsLeaderSwitchingEnabled
	    {
		    get => !IsLeaderNotRequired;
	    }

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

	    public Class Class
	    {
		    get => _class;
		    set => SetField(ref _class, value);
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

	    public ICommand AddClassCommand
	    {
		    get => new RelayCommand(AddClass);
	    }

	    private void AddClass(object parameter)
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
			    if (classRepository.IsClassNumberExists(Class.ClassNumber))
			    {
					_notifier.Notify("Такой класс уже существует");
					return;
			    }

			    if (IsLeaderNotRequired)
			    {
				    Class.Leader = null;
			    }

			    if (IsAuditoryNotRequired)
			    {
					Class.Auditory = null; 
			    }

			    var newClass = new Class()
			    {
				    ClassNumber = Class.ClassNumber,
				    AuditoryId = SelectedAuditory?.Id ?? null,
				    LeaderId = SelectedLeader?.Id ?? null
			    };

				classRepository.Add(newClass);

				if (!IsAuditoryNotRequired)
				{
					newClass.Auditory = SelectedAuditory;
					AvailableAuditories.Remove(SelectedAuditory);
				}

				if (!IsLeaderNotRequired)
				{
					newClass.Leader = SelectedLeader;
					AvailableEmployees.Remove(SelectedLeader);
				}

				WindowMessenger.OnMessageSend(new NewClassMessage() { Class = newClass});

				_notifier.Notify("Класс добавлен");
		    }
	    }


	    public AddNewClassWindowViewModel()
	    {
		    _notifier = new MessageBoxNotifier();
		    Class = new Class();

		    using (var context = new ApplicationContext())
		    {
			    AvailableAuditories = new ObservableCollection<Auditory>(
				    new AuditoriesRepository(context).GetFreeAuditories()
			    );
			    AvailableEmployees = new ObservableCollection<MyJournalLibrary.Entities.Employee>(
				    new EmployeesRepository(context).GetFreeClassEmployeesWithContacts()
			    );
		    }
	    }
    }
}
