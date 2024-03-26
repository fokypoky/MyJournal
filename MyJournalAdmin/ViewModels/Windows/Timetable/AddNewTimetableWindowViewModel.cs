using System;
using System.Collections.Generic;
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
using MyJournalLibrary.Entities;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournalAdmin.ViewModels.Windows.Timetable
{
	public class AddNewTimetableWindowViewModel : ViewModel
	{
		private INotifier _notifier;

		private List<string> _daysOfWeek = new() { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" };
		private string _selectedDayOfWeek;

		private string[] _lessonTimes = { "09:00", "09:55", "10:50", "11:45", "12:40", "13:35", "14:30", "15:25" };
		private string _selectedLessonTime;

		private ObservableCollection<Class> _classes;
		private Class _selectedClass;

		private ObservableCollection<Subject> _selectedClassSubjects;
		private Subject _selectedSubject;

		private ObservableCollection<Auditory> _availableAuditories;
		private Auditory _selectedAuditory;

		private ObservableCollection<MyJournalLibrary.Entities.Employee> _availableEmployees;
		private MyJournalLibrary.Entities.Employee _selectedEmployee;

		#region Public fields

		public MyJournalLibrary.Entities.Employee SelectedEmployee
		{
			get => _selectedEmployee;
			set => SetField(ref _selectedEmployee, value);
		}

		public Auditory SelectedAuditory
		{
			get => _selectedAuditory;
			set => SetField(ref _selectedAuditory, value);
		}

		public string SelectedLessonTime
		{
			get => _selectedLessonTime;
			set
			{
				SetField(ref _selectedLessonTime, value);

				if (String.IsNullOrEmpty(SelectedLessonTime))
				{
					return;
				}

				using (var context = new ApplicationContext())
				{
					AvailableAuditories = new(new AuditoriesRepository(context).GetAll());
					AvailableEmployees = new(
						(from es in new EmployeeSubjectRepository(context).GetEmployeesWithContactsBySubject(
								SelectedSubject)
							select es.Employee).ToList()
						);
				}
			}
		}

		public Class SelectedClass
		{
			get => _selectedClass;
			set
			{
				SetField(ref _selectedClass, value);

				using (var context = new ApplicationContext())
				{
					SelectedClasSubjects = new ObservableCollection<Subject>(
						new SubjectsRepository(context).GetByClass(SelectedClass)
					);
				}
			}
		}

		public Subject SelectedSubject
		{
			get => _selectedSubject;
			set => SetField(ref _selectedSubject, value);
		}

		public string SelectedDayOfWeek
		{
			get => _selectedDayOfWeek;
			set => SetField(ref _selectedDayOfWeek, value);
		}

		#endregion

		#region Public collections

		public string[] LessonTimes
		{
			get => _lessonTimes;
			set => SetField(ref _lessonTimes, value);
		}

		public List<string> DaysOfWeek
		{
			get => _daysOfWeek;
			set => SetField(ref _daysOfWeek, value);
		}

		public ObservableCollection<Class> Classes
		{
			get => _classes;
			set => SetField(ref _classes, value);
		}

		public ObservableCollection<Subject> SelectedClasSubjects
		{
			get => _selectedClassSubjects;
			set => SetField(ref _selectedClassSubjects, value);
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

		#endregion

		public ICommand AddTimetableCommand
		{
			get => new RelayCommand(AddTimetable);
		}

		private void AddTimetable(object parameter)
		{
			if (!ValidateInput())
			{
				_notifier.Notify("Проверьте корректность ввода");
				return;
			}

			using (var context = new ApplicationContext())
			{
				var timetable = new MyJournalLibrary.Entities.Timetable()
				{
					AuditoryId = SelectedAuditory.Id,
					ClassId = SelectedClass.Id,
					DayOfWeek = DaysOfWeek.IndexOf(SelectedDayOfWeek) + 1,
					LessonTime = new TimeOnly(int.Parse(SelectedLessonTime.Split(":")[0]),
						int.Parse(SelectedLessonTime.Split(":")[1])),
					SubjectId = SelectedSubject.Id,
					TeacherId = SelectedEmployee.Id
				};

				// Проверка не занято ли время у класса
				var timetableRepository = new TimetableRepository(context);

				if (!timetableRepository.IsClassTimeFree(SelectedClass, timetable.DayOfWeek, timetable.LessonTime))
				{
					_notifier.Notify("В указанное время у класса стоит урок");
					return;
				}

				timetableRepository.Add(timetable);
				WindowMessenger.OnMessageSend(new NewTimetableMessage()
				{
					Timetable = new MyJournalLibrary.Entities.Timetable()
					{
						Auditory = SelectedAuditory, 
						Class = SelectedClass,
						Subject = SelectedSubject, Teacher = SelectedEmployee,
						DayOfWeek = timetable.DayOfWeek, LessonTime = timetable.LessonTime
					}
				});
			}
		}

		private bool ValidateInput()
		{
			return SelectedClass is not null && SelectedSubject is not null
			                                 && SelectedDayOfWeek is not null &&
			                                 !String.IsNullOrEmpty(SelectedLessonTime)
			                                 && SelectedAuditory is not null && SelectedEmployee is not null;
		}
		
		public AddNewTimetableWindowViewModel()
		{
			_notifier = new MessageBoxNotifier();
			using (var context = new ApplicationContext())
			{
				Classes = new ObservableCollection<Class>(
					new ClassRepository(context).GetAll()
				);
			}
		}
	}
}
