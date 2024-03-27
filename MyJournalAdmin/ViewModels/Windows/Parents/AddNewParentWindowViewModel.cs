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
    public class AddNewParentWindowViewModel : ViewModel
    {
        private Contact _contacts;
        private ObservableCollection<Contact> _studentsContacts;
        private Contact _selectedStudentContacts;
        private string _studentToFindPhoneOrEmail;
        private INotifier _notifier;

        public string[] Genders { get; } =  { "М", "Ж" };
        public Contact Contacts
        {
            get => _contacts;
            set => SetField(ref _contacts, value);
        }

        public ObservableCollection<Contact> StudentsContacts
        {
            get => _studentsContacts;
            set => SetField(ref _studentsContacts, value);
        }

        public Contact SelectedStudentContacts
        {
            get => _selectedStudentContacts;
            set => SetField(ref _selectedStudentContacts, value);
        }

        public string StudentToFindPhoneOrEmail
        {
            get => _studentToFindPhoneOrEmail;
            set => SetField(ref _studentToFindPhoneOrEmail, value);
        }
        
        #region Commands

        public ICommand GeneratePasswordCommand
        {
            get => new RelayCommand(GeneratePassword);
        }

        public ICommand AddStudentCommand
        {
            get => new RelayCommand(AddStudent);
        }

        public ICommand RemoveStudentCommand
        {
            get => new RelayCommand(RemoveStudent);
        }

        public ICommand AddParentCommand
        {
            get => new RelayCommand(AddParent);
        }
        
        #endregion

        #region Command methods

        private void AddParent(object parameter)
        {
            if (!IsContactsValid(Contacts))
            {
                _notifier.Notify("Не все данные введены");
                return;
            }

            using (var context = new ApplicationContext())
            {
                var contactsRepository = new ContactsRepository(context);

                if (contactsRepository.IsPhoneNumberExists(Contacts.PhoneNumber))
                {
                    _notifier.Notify("Номер телефона уже используется");
                    return;
                }

                if (contactsRepository.IsEmailExists(Contacts.Email))
                {
                    _notifier.Notify("Email уже используется");
                    return;
                }
                
                contactsRepository.Add(Contacts);
                var parent = new Parent() { ContactsId = Contacts.Id };
                
                new ParentsRepository(context).Add(parent);

                if (StudentsContacts.Count > 0)
                {
                    var studentsRepository = new StudentsRepository(context);
                    var parentStudents = new List<ParentStudent>();
                    foreach (var studentContacts in StudentsContacts)
                    {
                        parentStudents.Add(new ParentStudent()
                            { StudentId = studentsRepository.GetByContacts(studentContacts).Id, ParentId = parent.Id });
                    }
                    
                    new ParentStudentRepository(context).AddRange(parentStudents);
                }
                
                WindowMessenger.OnMessageSend(new NewParentMessage() { NewParent = parent });
            }
            
            _notifier.Notify("Родитель добавлен");
        }
        
        private void GeneratePassword(object parameter)
        {
            Contacts.Password = new PasswordGenerator().Generate();
            OnPropertyChanged(nameof(Contacts));
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
                    StudentsContacts.Add(studentContacts);   
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
                StudentsContacts.Remove(SelectedStudentContacts);
            }
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
        
        public AddNewParentWindowViewModel()
        {
            _notifier = new MessageBoxNotifier();

            using (var context = new ApplicationContext())
            {

                Contacts = new Contact()
                {
                    UserRoleId = new UserRoleRepository(context).GetIdByRolename("parent")
                };
            }
            
            StudentsContacts = new ObservableCollection<Contact>();
            SelectedStudentContacts = new Contact();
        }
    }
}

