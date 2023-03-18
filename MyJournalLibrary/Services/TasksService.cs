using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournalLibrary.Services;

public class TasksService : TasksRepository
{
    public TasksService(DbContext context) : base(context)
    {
    }
}