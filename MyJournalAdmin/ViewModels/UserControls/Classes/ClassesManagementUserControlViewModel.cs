using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MyJournalAdmin.Infrastructure.Commands;
using MyJournalAdmin.Infrastructure.Repositories;
using MyJournalAdmin.ViewModels.Base;
using MyJournalAdmin.Views.Notifiers.Implementation;
using MyJournalAdmin.Views.Notifiers.Interfaces;
using MyJournalLibrary.Entities;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournalAdmin.ViewModels.UserControls.Classes
{
    public class ClassesManagementUserControlViewModel : ViewModel
    {
	    private INotifier _notifier;

		private ObservableCollection<Class> _classes;
		private Class _selectedClass;

		public Class SelectedClass
		{
			get => _selectedClass;
			set => SetField(ref _selectedClass, value);
		}

		public ObservableCollection<Class> Classes
		{
			get => _classes;
			set => SetField(ref _classes, value);
		}

		#region Commands

		public ICommand AddClassCommand
		{
			get => new RelayCommand(AddClass);
		}

		public ICommand RemoveClassCommand
		{
			get => new RelayCommand(RemoveClass);
		}

		public ICommand UpdateClassCommand
		{
			get => new RelayCommand(UpdateClass);
		}

		#endregion

		#region Command methods

		private void AddClass(object parameter) { }

		private void RemoveClass(object parameter)
		{
			if (SelectedClass is null)
			{
				_notifier.Notify("Класс не выбран");
				return;
			}

			using (var context = new ApplicationContext())
			{
				var classSubjectRepository = new ClassSubjectRepository(context);
				var studentsRepository = new StudentsRepository(context);
				var timetableRepository = new TimetableRepository(context);
				var tasksRepository = new TasksRepository(context);

				classSubjectRepository.RemoveAllByClass(SelectedClass);

				var students = studentsRepository.GetAllByClass(SelectedClass).ToList();
				foreach (var student in students)
				{
					var newStudent = new Student()
					{
						Id = student.Id,
						ContactsId = student.ContactsId,
						ClassId = null
					};

					studentsRepository.UpdateNoTracking(student, newStudent);
				}

				timetableRepository.RemoveAllByClass(SelectedClass);
				tasksRepository.RemoveAllByClass(SelectedClass);

				Classes.Remove(SelectedClass);
				SelectedClass = null;

				_notifier.Notify("Класс удален");
			}
		}

		private void UpdateClass(object parameter)
		{
			
		}

		#endregion

		public ClassesManagementUserControlViewModel()
		{
			_notifier = new MessageBoxNotifier();

		    using (var context = new ApplicationContext())
		    {
			    Classes = new ObservableCollection<Class>(
				    new ClassRepository(context).GetAllWithLeaderAndAuditory()
			    );
		    }
	    }
    }
}
