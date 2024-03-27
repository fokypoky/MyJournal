using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MyJournalAdmin.Infrastructure.Commands;
using MyJournalAdmin.Infrastructure.Repositories;
using MyJournalAdmin.Models;
using MyJournalAdmin.Models.Messenging;
using MyJournalAdmin.Models.Messenging.MessageTypes;
using MyJournalAdmin.ViewModels.Base;
using MyJournalAdmin.Views.Notifiers.Implementation;
using MyJournalAdmin.Views.Notifiers.Interfaces;
using MyJournalLibrary.Entities;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournalAdmin.ViewModels.Windows.Employee
{
    public class AddNewEmployeeWindowViewModel : ViewModel
    {
	    private MyJournalLibrary.Entities.Employee _newEmployee;

	    private string _selectedEmployeeSex;

	    private ObservableCollection<Class> _availableClasses;
	    private ObservableCollection<Class> _newEmployeeClasses;
	    private Class _selectedToRemoveClass; // ListView;
	    private Class _selectedToAddClass; // ComboBox

	    private ObservableCollection<Subject> _allSubjects;
	    private ObservableCollection<Subject> _newEmployeeSubjects;
	    private Subject _selectedToRemoveSubject; // ListView
	    private Subject _selectedToAddSubject; // ComboBox

	    private ObservableCollection<string> _genders = new ObservableCollection<string>()
	    {
		    "М", "Ж"
	    };

	    private INotifier _notifier;

	    private string _selectedGender;
		
	    #region Public fields

	    public string SelectedGender
	    {
		    get => _selectedGender;
		    set => SetField(ref _selectedGender, value);
	    }

	    public MyJournalLibrary.Entities.Employee NewEmployee
	    {
		    get => _newEmployee;
		    set => SetField(ref _newEmployee, value);
	    }

	    public Class SelectedToRemoveClass
	    {
		    get => _selectedToRemoveClass;
		    set => SetField(ref _selectedToRemoveClass, value);
	    }

	    public Class SelectedToAddClass
	    {
		    get => _selectedToAddClass;
		    set => SetField(ref _selectedToAddClass, value);
	    }

	    public Subject SelectedToRemoveSubject
	    {
		    get => _selectedToRemoveSubject; 
		    set => SetField(ref _selectedToRemoveSubject, value);
	    }

	    public Subject SelectedToAddSubject
	    {
			get => _selectedToAddSubject; 
			set => SetField(ref _selectedToAddSubject, value);
	    }

		#endregion

		#region Public collections

		public ObservableCollection<string> Genders
		{
			get => _genders;
			set => SetField(ref _genders, value);
		}

		public ObservableCollection<Class> AvailableClasses
		{
			get => _availableClasses;
			set => SetField(ref _availableClasses, value);
		}

		public ObservableCollection<Class> NewEmployeeClasses
		{
			get => _newEmployeeClasses;
			set => SetField(ref _newEmployeeClasses, value);
		}

		public ObservableCollection<Subject> AllSubjects
		{
			get => _allSubjects;
			set => SetField(ref _allSubjects, value);
		}

		public ObservableCollection<Subject> NewEmployeeSubjects
		{
			get => _newEmployeeSubjects;
			set => SetField(ref _newEmployeeSubjects, value);
		}

		#endregion

		#region Commands

		public ICommand AddClassCommand
		{
			get => new RelayCommand(AddClass);
		}

		public ICommand RemoveClassCommand
		{
			get => new RelayCommand(RemoveClass);
		}

		public ICommand AddSubjectCommand
		{
			get => new RelayCommand(AddSubject);
		}

		public ICommand RemoveSubjectCommand
		{
			get => new RelayCommand(RemoveSubject);
		}

		public ICommand GeneratePasswordCommand
		{
			get => new RelayCommand(GeneratePassword);
		}

		public ICommand AddEmployeeCommand
		{
			get => new RelayCommand(AddEmployee);
		}
		#endregion

		#region Command functions

		private void AddClass(object parameter)
		{
			if (SelectedToAddClass is null)
			{
				_notifier.Notify("Класс не выбран");
				return;
			}

			NewEmployeeClasses.Add(SelectedToAddClass);
			AvailableClasses.Remove(SelectedToAddClass);
			SelectedToAddClass = null;

		}

		private void RemoveClass(object parameter)
		{
			if (SelectedToRemoveClass is null)
			{
				_notifier.Notify("Класс не выбран");
				return;
			}

			AvailableClasses.Add(SelectedToRemoveClass);
			NewEmployeeClasses.Remove(SelectedToRemoveClass);
			SelectedToRemoveClass = null;

		}

		private void AddSubject(object parameter)
		{
			if (SelectedToAddSubject is null)
			{
				_notifier.Notify("Предмет не выбран");
				return;
			}

			NewEmployeeSubjects.Add(SelectedToAddSubject);
			AllSubjects.Remove(SelectedToAddSubject);
			SelectedToAddSubject = null;

		}

		private void RemoveSubject(object parameter)
		{
			if (SelectedToRemoveSubject is null)
			{
				_notifier.Notify("Предмет не выбран");
				return;
			}

			AllSubjects.Add(SelectedToRemoveSubject);
			NewEmployeeSubjects.Remove(SelectedToRemoveSubject);
			SelectedToAddSubject = null;

		}

		private void GeneratePassword(object parameter)
		{
			NewEmployee.Contacts.Password = new PasswordGenerator().Generate();
			OnPropertyChanged(nameof(NewEmployee));
		}

		private void AddEmployee(object parameter)
		{

			#region Input preparing

			NewEmployee.Contacts.Surname?.Replace(" ", "");
			NewEmployee.Contacts.Name?.Replace(" ", "");
			NewEmployee.Contacts.Midname?.Replace(" ", "");

			NewEmployee.Contacts.PhoneNumber?.Replace(" ", "");
			NewEmployee.Contacts.Email?.Replace(" ", "");

			NewEmployee.Contacts.Password?.Replace(" ", "");
			
			#endregion

			#region Input checkout

			if (String.IsNullOrEmpty(NewEmployee.Contacts.Surname) ||
			    String.IsNullOrEmpty(NewEmployee.Contacts.Name))
			{
				_notifier.Notify("Имя и фамилия не должны быть пустыми");
				return;
			}

			if (String.IsNullOrEmpty(NewEmployee.Contacts.PhoneNumber) ||
			                         String.IsNullOrEmpty(NewEmployee.Contacts.Email))
			{
				_notifier.Notify("Номер телефона и адрес электронной почты не должны быть пустыми");
				return;
			}

			if (String.IsNullOrEmpty(NewEmployee.Contacts.Password))
			{
				_notifier.Notify("Пароль не должен быть пустым");
				return;
			}

			if (String.IsNullOrEmpty(NewEmployee.Contacts.Sex))
			{
				_notifier.Notify("Укажите пол сотрудника");
				return;
			}

			using (var context = new ApplicationContext())
			{
				var contactsRepository = new ContactsRepository(context);

				if (contactsRepository.IsEmailExists(NewEmployee.Contacts.Email))
				{
					_notifier.Notify("Email занят");
					return;
				}

				if (contactsRepository.IsPhoneNumberExists(NewEmployee.Contacts.PhoneNumber))
				{
					_notifier.Notify("Номер телефона занят");
					return;
				}
			}

			#endregion
			
			// TODO: добавить хэширование пароля
			using (var context = new ApplicationContext())
			{
				var employeesRepository = new EmployeesRepository(context);
				employeesRepository.Add(NewEmployee);
				var employeeId = employeesRepository.GetIdByPhoneNumber(NewEmployee.Contacts.PhoneNumber);

				if (NewEmployeeSubjects.Count > 0 && employeeId is not null)
				{
					NewEmployee.Id = (int)employeeId;
					employeesRepository.AddSubjectsToEmployee(NewEmployee, NewEmployeeSubjects);
				}

				if (NewEmployeeClasses.Count > 0 && employeeId is not null)
				{
					NewEmployee.Id = (int)employeeId;

					foreach (var @class in NewEmployeeClasses)
					{
						@class.LeaderId = NewEmployee.Id;
					}

					new ClassRepository(context).UpdateRange(NewEmployeeClasses);
				}

				// TODO: id может быть null
				WindowMessenger.OnMessageSend(new NewEmployeeMessage() {Employee = NewEmployee});

				_notifier.Notify("Сотрудник добавлен");
			}

		}

		#endregion

		public AddNewEmployeeWindowViewModel()
		{
			_notifier = new MessageBoxNotifier();

		    // TODO: !!!!!!!!!! УБРАТЬ ЖЕСТКУЮ ПРИВЯЗКУ К АЙДИШНИКУ !!!!!!!!!!
			NewEmployee = new MyJournalLibrary.Entities.Employee()
		    {
			    Contacts = new Contact() { UserRoleId = 1 }
		    };

			using (var context = new ApplicationContext())
			{
				AllSubjects = new ObservableCollection<Subject>(
					new SubjectsRepository(context).GetAll()
				);

				AvailableClasses = new ObservableCollection<Class>(
					new ClassRepository(context).GetClassesWithoutLeader()
				);
			}

			NewEmployeeClasses = new ObservableCollection<Class>();
			NewEmployeeSubjects = new ObservableCollection<Subject>();

		}

    }
}
