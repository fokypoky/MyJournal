using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

namespace MyJournalAdmin.ViewModels.Windows.Students
{
    class AddNewStudentWindowViewModel : ViewModel
    {
	    private readonly INotifier _notifier;

	    private string[] _genders = { "М", "Ж" };
	    private Contact _contacts;
	    private string _parentToFindPhoneOrEmail;
		private ObservableCollection<Contact> _parentsContacts;
		private ObservableCollection<Class> _classes;
		private Contact _selectedParentContacts;
		private Class _selectedClass;

		#region Public fields

		public Contact Contacts
	    {
			get => _contacts;
			set => SetField(ref _contacts, value);
	    }

	    public string[] Genders
	    {
			get => _genders;
			set => SetField(ref _genders, value);
	    }

	    public string ParentToFindPhoneOrEmail
	    {
		    get => _parentToFindPhoneOrEmail;
		    set => SetField(ref _parentToFindPhoneOrEmail, value);
	    }

	    public Contact SelectedParentContacts
	    {
		    get => _selectedParentContacts;
		    set => SetField(ref _selectedParentContacts, value);
	    }

	    public Class SelectedClass
	    {
		    get => _selectedClass;
		    set => SetField(ref _selectedClass, value);
	    }

	    public ObservableCollection<Contact> ParentsContacts
	    {
		    get => _parentsContacts;
		    set => SetField(ref _parentsContacts, value);
	    }

	    public ObservableCollection<Class> Classes
	    {
		    get => _classes;
		    set => SetField(ref _classes, value);
	    }

	    #endregion

		#region Commands

		public ICommand AddStudentCommand
	    {
		    get => new RelayCommand(AddStudent);
	    }

	    public ICommand GeneratePasswordCommand
	    {
		    get => new RelayCommand(GeneratePassword);
	    }

	    public ICommand AddParentCommand
	    {
		    get => new RelayCommand(AddParent);
	    }

	    public ICommand RemoveParentCommand
	    {
		    get => new RelayCommand(RemoveParent);
	    }

		#endregion

		#region Command methods

		private void AddParent(object parameter)
		{
			if (String.IsNullOrEmpty(ParentToFindPhoneOrEmail) || String.IsNullOrWhiteSpace(ParentToFindPhoneOrEmail))
			{
				_notifier.Notify("Введите номер телефона или E-mail родителя");
				return;
			}

			using (var context = new ApplicationContext())
			{
				var contacts = new ContactsRepository(context)
					.GetByLoginAndRoleId(ParentToFindPhoneOrEmail,
						new UserRoleRepository(context).GetIdByRolename("parent"));
				
				if (contacts is null)
				{
					_notifier.Notify("Контакты не найдены");
					return;
				}

				var parent = new ParentsRepository(context).GetByContacts(contacts);
				if (parent is null)
				{
					_notifier.Notify("Родителя с такими контактами не существует");
					return;
				}

				ParentsContacts.Add(contacts);
			}
		}

		private void RemoveParent(object parameter)
		{
			if (SelectedParentContacts is null)
			{
				_notifier.Notify("Родитель не выбран");
				return;
			}

			ParentsContacts.Remove(SelectedParentContacts);
			SelectedParentContacts = null;
		}

		private void AddStudent(object parameter)
		{
			if (!IsContactsValid(Contacts) || SelectedClass is null)
			{
				_notifier.Notify("Указаны не все данные ученика");
				return;
			}

			using (var context = new ApplicationContext())
			{
				var contactsRepository = new ContactsRepository(context);
				
				if (contactsRepository.IsEmailExists(Contacts.Email))
				{
					_notifier.Notify("E-mail уже используется");
					return;
				}

				if (contactsRepository.IsPhoneNumberExists(Contacts.PhoneNumber))
				{
					_notifier.Notify("Номер телефона уже используется");
					return;
				}

				contactsRepository.Add(Contacts);

				var student = new Student() { ContactsId = Contacts.Id, ClassId = SelectedClass.Id };
				new StudentsRepository(context).Add(student);

				Debug.WriteLine($"STUDENT ID: {student.Id}");

				if (ParentsContacts.Count > 0)
				{
					var parentsRepository = new ParentsRepository(context);
					var parentStudents = new List<ParentStudent>();
					
					foreach (var parentsContact in ParentsContacts)
					{
						parentStudents.Add(new ParentStudent()
						{
							ParentId = parentsRepository.GetByContacts(parentsContact).Id,
							StudentId = student.Id
						});
					}
					
					new ParentStudentRepository(context).AddRange(parentStudents);
				}
				
				_notifier.Notify("Ученик добавлен");
				WindowMessenger.OnMessageSend(new NewStudentMessage() { NewStudent = student });
			}
		}

		private void GeneratePassword(object parameter)
		{
			Contacts.Password = new PasswordGenerator().Generate();
			OnPropertyChanged(nameof(Contacts));
		}

		#endregion

		private bool IsInputValid(string input)
		{
			return !String.IsNullOrWhiteSpace(input) && !String.IsNullOrEmpty(input);
		}

		private bool IsContactsValid(Contact contact)
		{
			return IsInputValid(contact.Surname) && IsInputValid(contact.Name)
			                                     && IsInputValid(contact.PhoneNumber) && IsInputValid(contact.Sex)
			                                     && IsInputValid(contact.Password) && IsInputValid(contact.Email);
		}

		public AddNewStudentWindowViewModel()
	    {
		    Contacts = new Contact();
		    _notifier = new MessageBoxNotifier();
		    ParentsContacts = new ObservableCollection<Contact>();

		    using (var context = new ApplicationContext())
		    {
			    Classes = new ObservableCollection<Class>(
				    new ClassRepository(context).GetAll()
			    );
			    Contacts.UserRoleId = new UserRoleRepository(context).GetIdByRolename("student");
		    }
	    }
    }
}
