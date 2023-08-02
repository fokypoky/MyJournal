using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MyJournal.Infrastructure.Commands;
using MyJournal.ViewModels.Base;
using MyJournalLibrary.Entities;
using Task = MyJournalLibrary.Entities.Task;

namespace MyJournal.ViewModels.UserControlsViewModels.Teacher
{
    public class TeacherTasksUserControlViewModel : ViewModel
    {
        private Task _selectedTask;

        private ObservableCollection<Task> _tasks;
        
        private ObservableCollection<Class> _classes;
        private ObservableCollection<Subject> _subjects;
        public Task SelectedTask
        {
            get => _selectedTask;
            set => SetField(ref _selectedTask, value);
        }
        #region Collections
        public ObservableCollection<Task> Tasks
        {
            get => _tasks;
            set => SetField(ref _tasks, value);
        }
        public ObservableCollection<Class> Classes
        {
            get => _classes;
            set => SetField(ref _classes, value);
        }
        public ObservableCollection<Subject> Subjects
        {
            get => _subjects;
            set => SetField(ref _subjects, value);
        }
        #endregion
        #region Commands
        public ICommand AddTaskCommand
        {
            get => new RelayCommand((object parameter) =>
            {

            });
        }
        public ICommand RemoveTaskCommand
        {
            get => new RelayCommand((object parameter) =>
            {

            });
        }
        public ICommand SaveChangesCommand
        {
            get => new RelayCommand((object parameter) =>
            {

            });
        }
        #endregion
        public TeacherTasksUserControlViewModel()
        {
            
        }
    }
}
