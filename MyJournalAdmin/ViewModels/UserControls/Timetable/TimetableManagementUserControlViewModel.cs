using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using MyJournalAdmin.Infrastructure.Commands;
using MyJournalAdmin.Infrastructure.Repositories;
using MyJournalAdmin.Models.Messenging;
using MyJournalAdmin.Models.Messenging.MessageTypes;
using MyJournalAdmin.ViewModels.Base;
using MyJournalAdmin.Views.Notifiers.Implementation;
using MyJournalAdmin.Views.Notifiers.Interfaces;
using MyJournalAdmin.Views.Windows.Timetable;
using MyJournalLibrary.Entities;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournalAdmin.ViewModels.UserControls.Timetable
{
    public class TimetableManagementUserControlViewModel : ViewModel
    {
	    private ObservableCollection<Class> _classes;
	    private ObservableCollection<MyJournalLibrary.Entities.Timetable> _classTimetables;

	    private ObservableCollection<MyJournalLibrary.Entities.Timetable> _mondayTimetables;
	    private ObservableCollection<MyJournalLibrary.Entities.Timetable> _tuesdayTimetables;
	    private ObservableCollection<MyJournalLibrary.Entities.Timetable> _wednesdayTimetables;
	    private ObservableCollection<MyJournalLibrary.Entities.Timetable> _thursdayTimetables;
	    private ObservableCollection<MyJournalLibrary.Entities.Timetable> _fridayTimetables;
	    private ObservableCollection<MyJournalLibrary.Entities.Timetable> _saturdayTimetables;
	    private ObservableCollection<MyJournalLibrary.Entities.Timetable> _sundayTimetables;

	    private MyJournalLibrary.Entities.Timetable _selectedTimetable;

		private Class _selectedClass;

		private INotifier _notifier;

		public MyJournalLibrary.Entities.Timetable SelectedTimetable
		{
			get => _selectedTimetable;
			set => SetField(ref _selectedTimetable, value);
		}

	    public Class SelectedClass
	    {
		    get => _selectedClass;
		    set
		    {
			    SetField(ref _selectedClass, value);
			    using (var context = new ApplicationContext())
			    {
				    ClassTimetables = new ObservableCollection<MyJournalLibrary.Entities.Timetable>(
					    new TimetableRepository(context).GetByClassWithEmployeeContactsSubjectAndAuditory(
						    _selectedClass)
				    );
			    }
			}
	    }

		#region Public collections

		public ObservableCollection<Class> Classes
		{
			get => _classes;
			set => SetField(ref _classes, value);
		}

		public ObservableCollection<MyJournalLibrary.Entities.Timetable> ClassTimetables
		{
			get => _classTimetables;
			set
			{
				SetField(ref _classTimetables, value);

				MondayTimetables = new(GetTimetablesByDayOfWeek(1).OrderBy(t => t.LessonTime));
				TuesdayTimetables = new(GetTimetablesByDayOfWeek(2).OrderBy(t => t.LessonTime));
				WednesdayTimetables = new(GetTimetablesByDayOfWeek(3).OrderBy(t => t.LessonTime));
				ThursdayTimetables = new(GetTimetablesByDayOfWeek(4).OrderBy(t => t.LessonTime));
				FridayTimetables = new(GetTimetablesByDayOfWeek(5).OrderBy(t => t.LessonTime));
				SaturdayTimetables = new(GetTimetablesByDayOfWeek(6).OrderBy(t => t.LessonTime));
				SundayTimetables = new(GetTimetablesByDayOfWeek(7).OrderBy(t => t.LessonTime));
			}
		}

		public ObservableCollection<MyJournalLibrary.Entities.Timetable> MondayTimetables
		{
			get => _mondayTimetables;
			set => SetField(ref _mondayTimetables, value);
		}

		public ObservableCollection<MyJournalLibrary.Entities.Timetable> TuesdayTimetables
		{
			get => _tuesdayTimetables;
			set => SetField(ref _tuesdayTimetables, value);
		}

		public ObservableCollection<MyJournalLibrary.Entities.Timetable> WednesdayTimetables
		{
			get => _wednesdayTimetables; 
			set => SetField(ref _wednesdayTimetables, value);
		}

		public ObservableCollection<MyJournalLibrary.Entities.Timetable> ThursdayTimetables
		{
			get => _thursdayTimetables;
			set => SetField(ref _thursdayTimetables, value);
		}

		public ObservableCollection<MyJournalLibrary.Entities.Timetable> FridayTimetables
		{
			get => _fridayTimetables;
			set => SetField(ref _fridayTimetables, value);
		}

		public ObservableCollection<MyJournalLibrary.Entities.Timetable> SaturdayTimetables
		{
			get => _saturdayTimetables;
			set => SetField(ref _saturdayTimetables, value);
		}

		public ObservableCollection<MyJournalLibrary.Entities.Timetable> SundayTimetables
		{
			get => _sundayTimetables;
			set => SetField(ref _sundayTimetables, value);
		}

		#endregion

		#region Commands

		public ICommand AddTimetableCommand
		{
			get => new RelayCommand(AddTimetable);
		}

		public ICommand RemoveTimetableCommand
		{
			get => new RelayCommand(RemoveTimetable);
		}

		public ICommand UpdateTimetableCommand
		{
			get => new RelayCommand(UpdateTimetable);
		}

		#endregion

		#region Command functions

		private void AddTimetable(object parameter)
		{
			new AddNewTimetableWindow().ShowDialog();
		}

		private void RemoveTimetable(object parameter)
		{
			if (SelectedTimetable is null)
			{
				_notifier.Notify("Выберите элемент расписания");
				return;
			}

			using (var context = new ApplicationContext())
			{
				new TimetableRepository(context).Remove(SelectedTimetable);
				var allTimetables = new ObservableCollection<MyJournalLibrary.Entities.Timetable>[]
				{
					MondayTimetables, TuesdayTimetables, WednesdayTimetables,
					ThursdayTimetables, FridayTimetables, SaturdayTimetables,
					SundayTimetables
				};

				var selectedTimetableCollection = allTimetables[SelectedTimetable.DayOfWeek - 1];
				selectedTimetableCollection.Remove(SelectedTimetable);

				_notifier.Notify("Элемент расписания удален");
			}
		}

		#endregion

		private List<MyJournalLibrary.Entities.Timetable> GetTimetablesByDayOfWeek(int dayOfWeek)
		{
			return ClassTimetables.Where(t => t.DayOfWeek == dayOfWeek).ToList();
		}

		private void OnMessageReceived(object? sender, EventArgs e)
		{
			if (SelectedClass is null)
			{
				return;
			}

			if (e is NewTimetableMessage)
			{
				var timetableMessage = (NewTimetableMessage)e;

				var timetableCollections = new List<ObservableCollection<MyJournalLibrary.Entities.Timetable>>()
				{
					MondayTimetables, TuesdayTimetables, WednesdayTimetables,
					ThursdayTimetables, FridayTimetables, SaturdayTimetables,
					SundayTimetables
				};

				var timetableCollection = timetableCollections.ElementAt(timetableMessage.Timetable.DayOfWeek - 1);
				timetableCollection.Add(timetableMessage.Timetable);
			}
		}

		public TimetableManagementUserControlViewModel()
		{
			WindowMessenger.MessageSender += OnMessageReceived;
			_notifier = new MessageBoxNotifier();
			
		    using (var context = new ApplicationContext())
		    {
			    Classes = new ObservableCollection<Class>(
				    new ClassRepository(context).GetAll()
			    );

			    ClassTimetables = new ObservableCollection<MyJournalLibrary.Entities.Timetable>();

			    MondayTimetables = new();
			    TuesdayTimetables = new();
			    WednesdayTimetables = new();
			    ThursdayTimetables = new();
			    FridayTimetables = new();
			    SaturdayTimetables = new();
			    SundayTimetables = new();
		    }
	    }
    }
}
