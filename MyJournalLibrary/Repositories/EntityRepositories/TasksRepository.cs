using Microsoft.EntityFrameworkCore;
using Task = MyJournalLibrary.Entities.Task;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class TasksRepository : EntityRepository<Task>
{
    public TasksRepository(DbContext context) : base(context)
    {
    }
}