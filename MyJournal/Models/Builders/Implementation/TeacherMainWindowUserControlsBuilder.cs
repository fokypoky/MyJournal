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

public class TeacherMainWindowUserControlsBuilder : MainWindowUserControlsBuilder
{
    public override void BuildMarksUserControl() => this.MarksUserControl = new TeacherSelectionMarksUserControl();
    
    public override void BuildProfileUserControl() => this.ProfileUserControl = new TeacherProfileUserControl();

    public override void BuildTasksUserControl() => this.TasksUserControl = new TeacherTasksUserControl();
    
    public override void BuildTimeTableUserControl() => this.TimeTableUserControl = new TeacherTimetableUserControl();
    
}
