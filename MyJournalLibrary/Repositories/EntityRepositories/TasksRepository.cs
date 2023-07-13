using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;
using Task = MyJournalLibrary.Entities.Task;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class TasksRepository : EntityRepository<Task>
{
    public TasksRepository(DbContext context) : base(context)
    {
    }

    public ICollection<Task> GetByClassSubjectAndPeriod(Class @class, Subject subject, int year, int month)
    {
        return _context.Set<Task>()
            .Where(t => t.ClassId == @class.Id && t.SubjectId == subject.Id)
            .ToList();

    }
}