using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyJournal.ViewModels.Base;
using MyJournalLibrary.Entities;
using Task = MyJournalLibrary.Entities.Task;

namespace MyJournal.ViewModels.UserControlsViewModels.Parent;

public class ParentTasksUserControlViewModel : ViewModel
{
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
		}
	}

	public int SelectedMonth
	{
		get => _selectedMonth;
		set
		{
			SetField(ref _selectedMonth, value);
		}
	}

	public Subject SelectedSubject
	{
		get => _selectedSubject;
		set
		{
			SetField(ref _selectedSubject,  value);
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
}
