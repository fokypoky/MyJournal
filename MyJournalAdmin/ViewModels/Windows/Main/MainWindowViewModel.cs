using System.Windows.Controls;
using MyJournalAdmin.ViewModels.Base;
using MyJournalAdmin.Views.UserControls.Auditories;
using MyJournalAdmin.Views.UserControls.Classes;
using MyJournalAdmin.Views.UserControls.Employees;
using MyJournalAdmin.Views.UserControls.Parents;
using MyJournalAdmin.Views.UserControls.Students;
using MyJournalAdmin.Views.UserControls.Subjects;
using MyJournalAdmin.Views.UserControls.Timetable;

namespace MyJournalAdmin.ViewModels.Windows.Main
{
    public class MainWindowViewModel : ViewModel
    {
	    public UserControl AuditoriesManagementUserControl
	    {
		    get => new AuditoriesManagementUserControl();
	    }

	    public UserControl ClassesManagementUserControl
	    {
		    get => new ClassesManagementUserControl();
	    }

	    public UserControl EmployeesManagementUserControl
	    {
		    get => new EmployeesManagementUserControl();
	    }

	    public UserControl StudentsManagementUserControl
	    {
		    get => new StudentsManagementUserControl();
	    }

	    public UserControl SubjectsManagementUserControl
	    {
		    get => new SubjectsManagementUserControl();
	    }

	    public UserControl TimetableManagementUserControl
	    {
		    get => new TimetableManagementUserControl();
	    }

	    public UserControl ParentsManagementUserControl
	    {
		    get => new ParentsManagementUserControl();
	    }

	}
}
