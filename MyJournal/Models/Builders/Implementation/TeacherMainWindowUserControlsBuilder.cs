using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MyJournal.Models.Builders.Interfaces;
using MyJournal.ViewModels.Controls;
using MyJournal.Views.UserControls.Teacher;

namespace MyJournal.Models.Builders.Implementation;

public class TeacherMainWindowUserControlsBuilder : Interfaces.MainWindowUserControlsBuilder
{
    public UserControl MainUserControl { get; set; }
    public UserControl ChatUserControl { get; set; }
    public UserControl MarksUserControl { get; set; }
    public UserControl ProfileUserControl { get; set; }
    public UserControl TasksUserControl { get; set; }
    public UserControl TimeTableUserControl { get; set; }
    public UserControl TimeTabletUserControl { get; set; }
    
    public override void BuildMarksUserControl() => MarksUserControl = new TeacherSelectionMarksUserControl();
    
    public override void BuildProfileUserControl() => ProfileUserControl = new TeacherProfileUserControl();

    public override void BuildTasksUserControl() => TasksUserControl = new TeacherTasksUserControl();
    
    public override void BuildTimeTableUserControl() => TimeTabletUserControl = new TeacherTimetableUserControl();
    
}
