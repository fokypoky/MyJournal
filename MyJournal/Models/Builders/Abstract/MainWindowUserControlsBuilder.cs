using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MyJournal.ViewModels.Controls;

namespace MyJournal.Models.Builders.Interfaces;

public abstract class MainWindowUserControlsBuilder
{
   public UserControl MainUserControl {get; set; }
   public UserControl ChatUserControl { get; set; }
   public UserControl MarksUserControl { get; set; }
   public UserControl ProfileUserControl { get; set; }
   public UserControl TasksUserControl { get; set; }
   public UserControl TimeTableUserControl { get; set; }
    public virtual void BuildMainUserControl() => MainUserControl = new MainUserControl();
    public virtual void BuildChatUserControl() => ChatUserControl = new ChatUserControl();
    public abstract void BuildMarksUserControl();
    public abstract void BuildProfileUserControl();
    public abstract void BuildTasksUserControl();
    public abstract void BuildTimeTableUserControl();
}