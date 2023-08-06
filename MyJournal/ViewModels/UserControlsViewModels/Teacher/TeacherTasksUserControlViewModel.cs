using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MyJournal.Infrastructure.Commands;
using MyJournal.Models;
using MyJournal.ViewModels.Base;
using MyJournal.Views.Windows;
using MyJournal.Views.Windows.Teacher;
using MyJournalLibrary.Entities;
using MyJournalLibrary.Repositories.EntityRepositories;
using Task = MyJournalLibrary.Entities.Task;

namespace MyJournal.ViewModels.UserControlsViewModels.Teacher
{
    public class TeacherTasksUserControlViewModel : ViewModel
    {
        private Task _selectedTask;

        private ObservableCollection<Task> _tasks;
        
        private ObservableCollection<Class> _classes;
        private ObservableCollection<Subject> _subjects;

        private ObservableCollection<int> _taskYears;
        private ObservableCollection<int> _taskMonths;

        private Class _selectedClass;
        private Subject _selectedSubject;

        private int _selectedYear;
        private int _selectedMonth;

        public int SelectedYear
        {
            get => _selectedYear;
            set
            {
                SetField(ref _selectedYear, value);
                LoadMonths();
            }
        }
        public int SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                SetField(ref _selectedMonth, value);
                LoadCurrentPeriodTasks();
            }
        }
        public Task SelectedTask
        {
            get => _selectedTask;
            set => SetField(ref _selectedTask, value);
        }
        public Subject SelectedSubject
        {
            get => _selectedSubject;
            set
            {
                SetField(ref _selectedSubject, value);
                LoadYears();
            }
        }
        public Class SelectedClass
        {
            get => _selectedClass;
            set
            {
                SetField(ref _selectedClass, value);
                LoadSubjects();
            }
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
        public ObservableCollection<int> TaskYears
        {
            get => _taskYears;
            set => SetField(ref _taskYears, value);
        }
        public ObservableCollection<int> TaskMonths
        {
            get => _taskMonths;
            set => SetField(ref _taskMonths, value);
        }
        #endregion
        #region Commands
        public ICommand AddTaskCommand
        {
            get => new RelayCommand((object parameter) =>
            {
                if (SelectedClass == null || SelectedSubject == null)
                {
                    MessageBox.Show("Сначала выберите класс и предмет");
                    return;
                }

                var window = new TeacherAddingTaskWindow();
                window.Show();

                WindowMessanger.OnMessageSend(new ClassSubjectMessage() {Class = SelectedClass, Subject = SelectedSubject});
            });
        }
        public ICommand RemoveTaskCommand
        {
            get => new RelayCommand((object parameter) =>
            {

            });
        }
        public ICommand UpdateTaskCommand
        {
            get => new RelayCommand((object parameter) =>
            {
                if (SelectedTask == null)
                {
                    MessageBox.Show("Задание не выбрано");
                    return;
                }

                var window = new TeacherEditingTaskWindow();
                window.Show();

                WindowMessanger.OnMessageSend(new TaskMessage() { Task = SelectedTask, Type = TaskMessageType.Send});
            });
        }
        public ICommand SaveChangesCommand
        {
            get => new RelayCommand((object parameter) =>
            {

            });
        }
        #endregion
        #region ComboBox collections loading
        private void LoadSubjects()
        {
            using (var context = new ApplicationContext())
            {
                Subjects = new ObservableCollection<Subject>(
                    new SubjectsRepository(context).GetByClass(SelectedClass)
                );
            }
        }
        private void LoadTaskDates()
        {
            using (var context = new ApplicationContext())
            {
                var tasksRepository = new TasksRepository(context);
            }
        }

        private void LoadYears()
        {
            using (var context = new ApplicationContext())
            {
                TaskYears = new ObservableCollection<int>(
                    new TasksRepository(context).GetTaskYearsByClassAndSubject(SelectedClass, SelectedSubject)
                );
            }
        }

        private void LoadMonths()
        {
            using (var context = new ApplicationContext())
            {
                TaskMonths = new ObservableCollection<int>(
                    new TasksRepository(context).GetTaskMonthByClassSubjectAndYear(SelectedClass, SelectedSubject,
                        SelectedYear)
                );
            }
        }
        #endregion

        private void LoadCurrentPeriodTasks()
        {
            using (var context = new ApplicationContext())
            {
                Tasks = new ObservableCollection<Task>(
                    new TasksRepository(context).GetByClassSubjectAndStartDatePeriod(SelectedClass, SelectedSubject,
                        SelectedYear, SelectedMonth)
                );
            }
        }

        private void OnMessageReceived(object sender, EventArgs e)
        {
            if (e is TaskMessage)
            {
                var message = (TaskMessage)e;
                switch (message.Type)
                {
                    case TaskMessageType.Add:
                        using (var context = new ApplicationContext())
                        {
                            var tasksRepository = new TasksRepository(context);
                            tasksRepository.Add(message.Task);
                        }
                        if (Tasks != null)
                        {
                            Tasks.Add(message.Task);
                        }
                        break;
                    case TaskMessageType.Edit:
                        using (var context = new ApplicationContext())
                        {
                            var tasksRepository = new TasksRepository(context);
                            tasksRepository.Update(message.Task);
                        }
                        break;
                }
            }
        }
        public TeacherTasksUserControlViewModel()
        {
            WindowMessanger.MessageSender += OnMessageReceived;
            using (var context = new ApplicationContext())
            {
                Classes = new ObservableCollection<Class>(
                    new ClassRepository(context).GetByEmployeeId(ApplicationData.UserId));
            }
        }
    }
}
