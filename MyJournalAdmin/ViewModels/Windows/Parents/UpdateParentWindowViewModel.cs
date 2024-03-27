using System;
using System.Collections.Generic;
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

namespace MyJournalAdmin.ViewModels.Windows.Parents
{
	public class UpdateParentWindowViewModel : ViewModel
	{
		private INotifier _notifier;

		private Contact _contacts;

		private string[] _genders = { "М", "Ж" };

		private ObservableCollection<Contact> _studentsContacts;
		private Contact _selectedStudentContacts;

		private string _studentToFindPhoneOrEmail;

		#region Public fields

		public string StudentToFindPhoneOrEmail
		{
			get => _studentToFindPhoneOrEmail;
			set => SetField(ref _studentToFindPhoneOrEmail, value);
		}

		public Contact SelectedStudentContacts
		{
			get => _selectedStudentContacts;
			set => SetField(ref _selectedStudentContacts, value);
		}

		public Contact Contacts
		{
			get => _contacts;
			set => SetField(ref _contacts, value);
		}

		#endregion

		#region Public collections

		public ObservableCollection<Contact> StudentsContacts
		{
			get => _studentsContacts;
			set => SetField(ref _studentsContacts, value);
		}

		public string[] Genders
		{
			get => _genders;
			set => SetField(ref _genders, value);
		}

		#endregion

		#region Commands

		public ICommand UpdateParentCommand
		{
			get => new RelayCommand(UpdateParent);
		}

		public ICommand AddStudentCommand
		{
			get => new RelayCommand(AddStudent);
		}

		public ICommand RemoveStudentCommand
		{
			get => new RelayCommand(RemoveStudent);
		}

		public ICommand GeneratePasswordCommand
		{
			get => new RelayCommand(GeneratePassword);
		}

		#endregion

		#region Commands methods

		private void UpdateParent(object parameter)
		{
			if (!IsContactsValid(Contacts))
			{
				_notifier.Notify("Не все данные введены");
				return;
			}

			using (var context = new ApplicationContext())
			{
				var contactsRepository = new ContactsRepository(context);

				// FIXME: при старом номере телефона/email возникает ошибка, связанная с уникальностью
				if (contactsRepository.IsPhoneNumberExists(Contacts.PhoneNumber) && contactsRepository.GetPhoneNumberById(Contacts.Id) != Contacts.PhoneNumber)
				{
					_notifier.Notify("Номер телефона уже используется");
					return;
				}

				if (contactsRepository.IsEmailExists(Contacts.Email) && contactsRepository.GetEmailById(Contacts.Id) != Contacts.Email)
				{
					_notifier.Notify("Email уже используется");
					return;
				}

				new ContactsRepository(context).Update(_contacts);
				_notifier.Notify("Контакты обновлены");
			}

		}

		private void AddStudent(object parameter)
		{
			using (var context = new ApplicationContext())
			{
				var studentContacts = new ContactsRepository(context).GetByLoginAndRoleId(
					StudentToFindPhoneOrEmail, new UserRoleRepository(context).GetIdByRolename("student")
				);

				if (studentContacts is null)
				{
					_notifier.Notify("Ученик с такими контактами не найден");
					return;
				}

				if (!StudentsContacts.Contains(studentContacts))
				{
					var student = new StudentsRepository(context).GetByContacts(studentContacts);
					var parent = new ParentsRepository(context).GetByContacts(Contacts);

					new ParentStudentRepository(context).Add(new ParentStudent()
						{ StudentId = student.Id, ParentId = parent.Id });


					StudentsContacts.Add(studentContacts);
					_notifier.Notify("Ученик добавлен");
				}
				else
				{
					_notifier.Notify("Ученик уже добавлен");
				}
			}
		}

		private void RemoveStudent(object parameter)
		{
			if (SelectedStudentContacts is null)
			{
				_notifier.Notify("Контакты ученика не выбраны");
				return;
			}

			if (StudentsContacts.Contains(SelectedStudentContacts))
			{
				using (var context = new ApplicationContext())
				{
					var parentStudentReository = new ParentStudentRepository(context);

					var parentStudent =
						parentStudentReository.GetByParentAndStudentContacts(Contacts, SelectedStudentContacts);

					parentStudentReository.Remove(parentStudent);

					StudentsContacts.Remove(SelectedStudentContacts);
					SelectedStudentContacts = null;

					_notifier.Notify("Ученик удален");
				}
			}
		}

		private void GeneratePassword(object parameter)
		{
			Contacts.Password = new PasswordGenerator().Generate();
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

		private void OnMessageReceived(object? sender, EventArgs e)
		{
			if (e is ParentToUpdateMessage)
			{
				var parentMessage = (ParentToUpdateMessage)e;
				Contacts = parentMessage.Parent.Contacts;

				using (var context = new ApplicationContext())
				{
					var parentStudentRepository = new ParentStudentRepository(context);
					StudentsContacts = new(
						(from ps in parentStudentRepository.GetWithStudentContactsByParentContacts(Contacts)
							select ps.Student.Contacts).ToList()
					);
				}
			}
		}

		public UpdateParentWindowViewModel()
		{
			_notifier = new MessageBoxNotifier();
			WindowMessenger.MessageSender += OnMessageReceived;
		}
	}
}