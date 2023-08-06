using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyJournal.Models.Builders.Implementation;
using MyJournal.Models.Builders.Interfaces;

namespace MyJournal.Models.Builders.Directors;

public class MainWindowUserControlsBuilderDirector
{
    public MainWindowUserControlsBuilder Builder { get; set; }
    
    public void Construct()
    {
        Builder.BuildMainUserControl();
        Builder.BuildChatUserControl();
        Builder.BuildMarksUserControl();
        Builder.BuildProfileUserControl();
        Builder.BuildTasksUserControl();
        Builder.BuildTimeTableUserControl();
    }
    public MainWindowUserControlsBuilderDirector(UserRole userRole)
    {
        switch (userRole)
        {
            case UserRole.Employee:
                Builder = new TeacherMainWindowUserControlsBuilder();
                break;
            case UserRole.Parent:
                break;
            case UserRole.Student:
                break;
            default:
                break;
        }
    }
}