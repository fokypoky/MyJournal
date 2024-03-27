using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyJournal.Models.Builders.Interfaces;
using MyJournal.Views.UserControls.Parent;

namespace MyJournal.Models.Builders.Implementation;

public class ParentMainWindowUserControlsBuilder : MainWindowUserControlsBuilder
{
    public override void BuildMarksUserControl() => MarksUserControl = new ParentMarksUserControl();
    public override void BuildProfileUserControl() => ProfileUserControl = new ParentProfileUserControl();

    public override void BuildTasksUserControl() => TasksUserControl = new ParentTasksUserControl();

    public override void BuildTimeTableUserControl() => TimeTableUserControl = new ParentTimetableUserControl();
}