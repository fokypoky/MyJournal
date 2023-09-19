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

namespace MyJournal.ViewModels.UserControlsViewModels.Parent;

public class ParentMarksUserControlViewModel : ViewModel
{
	private MyJournalLibrary.Entities.Parent _parent;
	private MyJournalLibrary.Entities.Student _selectedStudent;
	private Subject _selectedSubject;
	private int _selectedYear;
	private int _selectedMonth;

	private ObservableCollection<MyJournalLibrary.Entities.Student> _students;
	private ObservableCollection<Subject> _selectedStudentSubjects;
	private ObservableCollection<int> _selectedSubjectYears;
	private ObservableCollection<int> _selectedSubjectMonths;
	private ObservableCollection<Mark> _selectedStudentMarks;

	////////////////////////////////
	
	#region Public fields

	public MyJournalLibrary.Entities.Parent Parent
	{
		get => _parent; 
		set => SetField(ref _parent, value);
	}

	public MyJournalLibrary.Entities.Student SelectedStudent
	{
		get => _selectedStudent;
		set
		{
			SetField(ref _selectedStudent, value);
			
			SelectedStudentSubjects?.Clear();
			SelectedSubjectYears?.Clear();
			SelectedSubjectMonths?.Clear();
			SelectedStudentMarks?.Clear();

			FillSubjectsCollection();
		}
	}

	public Subject SelectedSubject
	{
		get => _selectedSubject;
		set
		{
			SetField(ref _selectedSubject, value);

			SelectedSubjectYears?.Clear();
			SelectedSubjectMonths?.Clear();
			SelectedStudentMarks?.Clear();

			FillYearsCollection();
		}
	}

	public int SelectedYear
	{
		get => _selectedYear;
		set
		{
			SetField(ref _selectedYear, value);

			SelectedSubjectMonths?.Clear();
			SelectedStudentMarks?.Clear();

			FillMonthsCollection();
		}
	}

	public int SelectedMonth
	{
		get => _selectedMonth;
		set
		{
			SetField(ref _selectedMonth, value);

			SelectedStudentMarks?.Clear();

			FillSelectedStudentMarksCollection();
		}
	}

	#endregion

	////////////////////////////////
	
	#region Public collections

	public ObservableCollection<MyJournalLibrary.Entities.Student> Students
	{
		get => _students;
		set => SetField(ref _students, value);
	}

	public ObservableCollection<Subject> SelectedStudentSubjects
	{
		get => _selectedStudentSubjects;
		set => SetField(ref _selectedStudentSubjects, value);
	}

	public ObservableCollection<int> SelectedSubjectYears
	{
		get => _selectedSubjectYears;
		set => SetField(ref _selectedSubjectYears, value);
	}

	public ObservableCollection<int> SelectedSubjectMonths
	{
		get => _selectedSubjectMonths;
		set => SetField(ref _selectedSubjectMonths, value);
	}

	public ObservableCollection<Mark> SelectedStudentMarks
	{
		get => _selectedStudentMarks;
		set => SetField(ref _selectedStudentMarks, value);
	}

	#endregion

	////////////////////////////////

	#region Collections filling

	private void FillSubjectsCollection()
	{
		if (SelectedStudent is null)
		{
			return;
		}

		using (var context = new ApplicationContext())
		{
			SelectedStudentSubjects = new ObservableCollection<Subject>(
				new SubjectsRepository(context).GetByStudent(SelectedStudent)
			);
		}
	}

	private void FillYearsCollection()
	{
		if (SelectedStudent is null || SelectedSubject is null)
		{
			return;
		}

		using (var context = new ApplicationContext())
		{
			SelectedSubjectYears = new ObservableCollection<int>(
				new MarksRepository(context).GetMarkYearsByStudentAndSubject(SelectedStudent, SelectedSubject)
			);

			if (!SelectedSubjectYears.Contains(DateTime.Now.Year))
			{
				SelectedSubjectYears.Add(DateTime.Now.Year);
			}

			SelectedSubjectYears.OrderDescending();
		}
	}

	private void FillMonthsCollection()
	{
		if (SelectedStudent is null || SelectedSubject is null || SelectedYear == 0)
		{
			return;
		}

		using (var context = new ApplicationContext())
		{
			SelectedSubjectMonths = new ObservableCollection<int>(
				new MarksRepository(context).GetMarkMonthsByStudentSubjectAndYear(SelectedStudent, SelectedSubject,
					SelectedYear)
			);

			if (!SelectedSubjectMonths.Contains(DateTime.Now.Month))
			{
				SelectedSubjectMonths.Add(DateTime.Now.Month);
			}

			SelectedSubjectMonths.OrderDescending();
		}
	}

	private void FillSelectedStudentMarksCollection()
	{
		if (SelectedStudent is null || SelectedSubject is null || SelectedYear == 0 || SelectedMonth == 0)
		{
			return;
		}

		using (var context = new ApplicationContext())
		{
			SelectedStudentMarks = new ObservableCollection<Mark>(
				new MarksRepository(context).GetWithTeacherAndContactsByStudentSubjectAndPeriod(
					SelectedStudent, SelectedSubject, SelectedYear, SelectedMonth
				)
			);
		}
	}

	#endregion

	private void ClearCollections(IEnumerable<ObservableCollection<object>> collections){}

	public ParentMarksUserControlViewModel()
	{
		using (var context = new ApplicationContext())
		{
			_parent =
				new ParentsRepository(context)
					.GetByUserIdWithContactsStudentsAndStudentContacts(ApplicationData.UserId);
		}
	}
}