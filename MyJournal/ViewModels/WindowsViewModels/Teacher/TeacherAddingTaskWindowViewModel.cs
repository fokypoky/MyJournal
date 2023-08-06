using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MyJournal.Infrastructure.Commands;
using MyJournal.Models;
using MyJournal.ViewModels.Base;
using MyJournalLibrary.Entities;
using Task = MyJournalLibrary.Entities.Task;

namespace MyJournal.ViewModels.WindowsViewModels.Teacher;

public class TeacherAddingTaskWindowViewModel : ViewModel
{
    private Class _class;
    private Subject _subject;

    private string _taskText;

    private DateTime _startDate;
    private DateTime _endDate;

    public DateTime StartDate
    {
        get => _startDate;
        set => SetField(ref _startDate, value);
    }

    public DateTime EndDate
    {
        get => _endDate;
        set => SetField(ref _endDate, value);
    }
    public string TaskText
    {
        get => _taskText;
        set => SetField(ref _taskText, value);
    }

    #region Commands
    public ICommand AddTaskCommand
    {
        get => new RelayCommand((object parameter) =>
        {
            if (String.IsNullOrEmpty(TaskText))
            {
                MessageBox.Show("Текст задания пустой");
                return;
            }

            if (StartDate == null && EndDate == null)
            {
                MessageBox.Show("Даты не выбраны");
                return;
            }

            var task = new Task()
            {
                ClassId = _class.Id, SubjectId = _subject.Id, TeacherId = ApplicationData.UserId,
                TaskText = this.TaskText, StartDate = this.StartDate, EndDate = this.EndDate
            };

            WindowMessanger.OnMessageSend(new TaskMessage() { Task = task, Type = TaskMessageType.Add});
        });
    }
    #endregion

    private void OnMessageReceived(object sender, EventArgs e)
    {
        if (e is ClassSubjectMessage)
        {
            var message = (ClassSubjectMessage)e;
            
            _class = message.Class;
            _subject = message.Subject;

            WindowMessanger.MessageSender -= OnMessageReceived;
        }
    }
    public TeacherAddingTaskWindowViewModel()
    {
        WindowMessanger.MessageSender += OnMessageReceived;
        
        var currentDate = DateTime.Now;
        StartDate = currentDate;
        EndDate = currentDate;
    }
}