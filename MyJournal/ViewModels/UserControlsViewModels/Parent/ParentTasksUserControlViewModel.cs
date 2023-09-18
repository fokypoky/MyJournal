using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyJournal.Models;
using MyJournal.ViewModels.Base;
using MyJournalLibrary.Entities;
using MyJournalLibrary.Repositories.EntityRepositories;
using Task = MyJournalLibrary.Entities.Task;

namespace MyJournal.ViewModels.UserControlsViewModels.Parent;

public class ParentTasksUserControlViewModel : ViewModel
{
	private MyJournalLibrary.Entities.Parent _parent;

	private MyJournalLibrary.Entities.Student _selectedStudent;
	private Task _selectedTask;
	private int _selectedYear;
	private int _selectedMonth;
	private Subject _selectedSubject;

	private ObservableCollection<MyJournalLibrary.Entities.Student> _students;
	private ObservableCollection<Task> _tasks;
	private ObservableCollection<Task> _deadlineTasks;
	private ObservableCollection<int> _taskYears;
	private ObservableCollection<int> _taskMonths;
	private ObservableCollection<Subject> _studentSubjects;


	#region Public fields

	public MyJournalLibrary.Entities.Student SelectedStudent
	{
		get => _selectedStudent;
		set
		{
			SetField(ref _selectedStudent, value);
			FillSubjectsComboBox();
		}
	}

	public Task SelectedTask
	{
		get => _selectedTask;
		set
		{
			SetField(ref _selectedTask, value);
		}
	}

	public int SelectedYear
	{
		get => _selectedYear;
		set
		{
			SetField(ref _selectedYear, value);
			FillMonthsComboBox();
		}
	}

	public int SelectedMonth
	{
		get => _selectedMonth;
		set
		{
			SetField(ref _selectedMonth, value);
			FillTasksData();
		}
	}

	public Subject SelectedSubject
	{
		get => _selectedSubject;
		set
		{
			SetField(ref _selectedSubject,  value);
			FillYearsComboBox();
		}
	}

	#endregion

	#region Public collections

	public ObservableCollection<MyJournalLibrary.Entities.Student> Students
	{
		get => _students;
		set => SetField(ref _students, value);
	}

	public ObservableCollection<Task> Tasks
	{
		get => _tasks;
		set => SetField(ref _tasks, value);
	}

	public ObservableCollection<Task> DeadlineTasks
	{
		get => _deadlineTasks;
		set => SetField(ref _deadlineTasks, value);
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

	public ObservableCollection<Subject> StudentSubjects
	{
		get => _studentSubjects;
		set => SetField(ref _studentSubjects, value);
	}

	#endregion

	#region ComboBox filling

	private void FillSubjectsComboBox()
	{
		if (SelectedStudent is null)
		{
			return;
		}

		using (var context = new ApplicationContext())
		{
			StudentSubjects = new ObservableCollection<Subject>(
				new SubjectsRepository(context).GetByStudent(SelectedStudent)
			);
		}
	}

	private void FillYearsComboBox()
	{
		if (SelectedSubject is null || SelectedSubject is null)
		{
			return;
		}

		using (var context = new ApplicationContext())
		{
			TaskYears = new ObservableCollection<int>(
				new TasksRepository(context).GetTaskYearsByStudentAndSubject(SelectedStudent, SelectedSubject)
			);
		}
	}

	private void FillMonthsComboBox()
	{
		if (SelectedStudent is null || SelectedSubject is null || SelectedYear == 0)
		{
			return;
		}

		using (var context = new ApplicationContext())
		{
			TaskMonths = new ObservableCollection<int>(
				new TasksRepository(context).GetTaskMonthByStudentSubjectAndYear(SelectedStudent, SelectedSubject,
					SelectedYear)
			);
		}
	}

	private void FillTasksData()
	{
		if (SelectedStudent is null || SelectedSubject is null || SelectedYear == 0 || SelectedMonth == 0)
		{
			return;
		}

		using (var context = new ApplicationContext())
		{
			var tasksRepository = new TasksRepository(context);

			Tasks = new ObservableCollection<Task>(
				tasksRepository.GetByStudentSubjectAndPeriod(SelectedStudent, SelectedSubject,
					SelectedYear, SelectedMonth)
			);
			DeadlineTasks = new ObservableCollection<Task>(
				tasksRepository.GetExpiringTasksByStudentSubjectAndDate(SelectedStudent, SelectedSubject, DateTime.Now)
			);
		}
	}

	#endregion

	public ParentTasksUserControlViewModel()
	{
		using (var context = new ApplicationContext())
		{
			_parent = new ParentsRepository(context).GetByUserId(ApplicationData.UserId);

			Students = new ObservableCollection<MyJournalLibrary.Entities.Student>(
				new StudentsRepository(context).GetWithContactsByParent(_parent)
			);
		}
	}
}
