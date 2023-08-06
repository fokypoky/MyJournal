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
using Task = MyJournalLibrary.Entities.Task;

namespace MyJournal.ViewModels.WindowsViewModels.Teacher;

public class TeacherEditingTaskWindowViewModel : ViewModel
{
    private Task _task;

    public Task Task
    {
        get => _task;
        set => SetField(ref _task, value);
    }

    #region Commands
    public ICommand UpdateTaskCommand
    {
        get => new RelayCommand((object parameter) =>
        {
            if (String.IsNullOrEmpty(Task.TaskText))
            {
                MessageBox.Show("Текст задания пустой");
                return;
            }

            if (Task.StartDate == null || Task.EndDate == null)
            {
                MessageBox.Show("Даты задания не выбраны");
                return;
            }

            WindowMessanger.OnMessageSend(new TaskMessage(){Task = Task, Type = TaskMessageType.Edit});
        });
    }
    #endregion

    private void OnMessageReceived(object sender, EventArgs e)
    {
        if (e is TaskMessage)
        {
            var message = (TaskMessage)e;
            Task = message.Task;

            WindowMessanger.MessageSender -= OnMessageReceived;
        }
    }
    public TeacherEditingTaskWindowViewModel()
    {
        WindowMessanger.MessageSender += OnMessageReceived;
    }
}
