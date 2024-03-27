using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MyJournalAdmin.Infrastructure.Commands;
using MyJournalAdmin.Infrastructure.Repositories;
using MyJournalAdmin.Models.Messenging;
using MyJournalAdmin.Models.Messenging.MessageTypes;
using MyJournalAdmin.ViewModels.Base;
using MyJournalAdmin.Views.Notifiers.Implementation;
using MyJournalAdmin.Views.Notifiers.Interfaces;
using MyJournalAdmin.Views.Windows.Parents;
using MyJournalLibrary.Entities;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournalAdmin.ViewModels.UserControls.Parents
{
    public class ParentsManagementUserControlViewModel : ViewModel
    {
        private INotifier _notifier;

        private ObservableCollection<Parent> _parents;
        private Parent? _selectedParent;

        #region Public fields

        public Parent? SelectedParent
        {
            get => _selectedParent;
            set => SetField(ref _selectedParent, value);
        }

        #endregion
        
        #region Collections

        public ObservableCollection<Parent> Parents
        {
            get => _parents;
            set => SetField(ref _parents, value);
        }

        #endregion"

        #region Commands

        public ICommand AddParentCommand
        {
            get => new RelayCommand(AddParent);
        }

        public ICommand RemoveParentCommand
        {
            get => new RelayCommand(RemoveParent);
        }

        public ICommand UpdateParentCommand
        {
            get => new RelayCommand(UpdateParent);
        }

        public ICommand RefreshCommand
        {
	        get => new RelayCommand(Refresh);
        }

        #endregion

        #region Command methods

        private void Refresh(object parameter)
        {
	        using (var context = new ApplicationContext())
	        {
		        Parents = new ObservableCollection<Parent>(
			        new ParentsRepository(context).GetAllWithContacts()
				        .OrderBy(p => p.Contacts.Surname)
		        );
		        SelectedParent = null;
	        }
		}

        private void AddParent(object parameter)
        {
            new AddNewParentWindow().ShowDialog();
        }

        private void RemoveParent(object parameter)
        {
            if (SelectedParent is null)
            {
                _notifier.Notify("Выберите родителя");
                return;
            }

            using (var context = new ApplicationContext())
            {
                var parentStudentRepository = new ParentStudentRepository(context);
                var parentStudents = parentStudentRepository.GetByParent(SelectedParent);
                parentStudentRepository.RemoveRange(parentStudents);
                
                new ParentsRepository(context).Remove(SelectedParent);
                new ContactsRepository(context).Remove(SelectedParent.Contacts);
                
                _notifier.Notify("Выбранный родитель удален");
                Parents.Remove(SelectedParent);

                SelectedParent = null;
            }
        }

        private void UpdateParent(object parameter)
        {
	        if (SelectedParent is null)
	        {
                _notifier.Notify("Выберите родителя");
                return;
	        }

	        new UpdateParentWindow().Show();
            WindowMessenger.OnMessageSend(new ParentToUpdateMessage() { Parent = SelectedParent });
        }

        #endregion

        private void OnMessageReceived(object? sender, EventArgs e)
        {
            if (e is NewParentMessage)
            {
                var parentMessage = (NewParentMessage)e;
                Parents.Add(parentMessage.NewParent);
                Parents.OrderBy(p => p.Contacts.Surname);
            }
        }
        
        public ParentsManagementUserControlViewModel()
        {
            WindowMessenger.MessageSender += OnMessageReceived;
            
            _notifier = new MessageBoxNotifier();
            using (var context = new ApplicationContext())
            {
                Parents = new ObservableCollection<Parent>(
                    new ParentsRepository(context).GetAllWithContacts()
                        .OrderBy(p => p.Contacts.Surname)
                );
            }
        }
    }
}