using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyJournal.Models.Builders.Interfaces;
using MyJournal.Views.UserControls.Student;

namespace MyJournal.Models.Builders.Implementation;

public class StudentMainWindowUserControlsBuilder : MainWindowUserControlsBuilder
{
    public override void BuildMarksUserControl() => this.MarksUserControl = new StudentMarksUserControl();

    public override void BuildProfileUserControl() => this.ProfileUserControl = new StudentProfileUserControl();

    public override void BuildTasksUserControl() => this.TasksUserControl = new StudentTasksUserControl();

    public override void BuildTimeTableUserControl() => this.TimeTableUserControl = new StudentTimetableUserControl();
}
