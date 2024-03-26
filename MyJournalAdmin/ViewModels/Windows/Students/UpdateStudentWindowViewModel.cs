using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
	public class UpdateStudentWindowViewModel : ViewModel
	{
		private INotifier _notifier;
		private Contact _contacts;
		private Contact _previousContact;

		private string[] _genders = {"М", "Ж"};
		
		private ObservableCollection<Class> _classes;
		private Class _selectedClass;

		private ObservableCollection<Contact> _parentsContacts;
		private Contact _selectedParentContacts;

		private string _parentToFindPhoneOrEmail;

		#region Public fields

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

		public Contact Contacts
		{
			get => _contacts;
			set => SetField(ref _contacts, value);
		}

		#endregion

		#region Public collections

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

		public string[] Genders
		{
			get => _genders;
			set => SetField(ref _genders, value);
		}

		#endregion

		#region Commands

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

		public ICommand UpdateStudentCommand
		{
			get => new RelayCommand(UpdateStudent);
		}

		#endregion

		#region Command functions

		private void UpdateStudent(object parameter)
		{

		}

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

				var student = new StudentsRepository(context).GetByContacts(Contacts);

				new ParentStudentRepository(context).Add(new ParentStudent() { ParentId = parent.Id, StudentId = student.Id});

				ParentsContacts.Add(contacts);

				_notifier.Notify("Родитель добавлен");
			}
		}

		private void RemoveParent(object parameter)
		{
			if (SelectedParentContacts is null)
			{
				_notifier.Notify("Родитель не выбран");
				return;
			}

			using (var context = new ApplicationContext())
			{
				var parentStudentRepository = new ParentStudentRepository(context);
				
				var parentStudent =
					parentStudentRepository.GetByParentAndStudentContacts(SelectedParentContacts, Contacts);
				parentStudentRepository.Remove(parentStudent);

				ParentsContacts.Remove(SelectedParentContacts);
			}

			SelectedParentContacts = null;

			_notifier.Notify("Родитель удален");
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

		private void OnMessageReceived(object? sender, EventArgs e)
		{
			if (e is StudentToUpdateMessage)
			{
				var studentMessage = (StudentToUpdateMessage)e;

				Contacts = studentMessage.Student.Contacts;

				using (var context = new ApplicationContext())
				{
					Classes = new ObservableCollection<Class>(
						new ClassRepository(context).GetAll()
					);
					SelectedClass = Classes.FirstOrDefault(c => c.Id == studentMessage.Student.ClassId);
					ParentsContacts = new ObservableCollection<Contact>(
						(from parent in new ParentStudentRepository(context).GetWithContactsByStudent(studentMessage
								.Student)
							select parent.Parent.Contacts).ToList()
					);
				}

				WindowMessenger.MessageSender -= OnMessageReceived;
			}
		}

		public UpdateStudentWindowViewModel()
		{
			WindowMessenger.MessageSender += OnMessageReceived;
			_contacts = new Contact();
			_notifier = new MessageBoxNotifier();
			ParentsContacts = new();
			_previousContact = new Contact();
		}
	}
}
