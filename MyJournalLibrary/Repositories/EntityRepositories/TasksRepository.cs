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
            .Where(t => t.ClassId == @class.Id && t.SubjectId == subject.Id
                                               && ((t.StartDate.Year == year && t.StartDate.Month == month) ||
                                                   (t.EndDate.Year == year && t.EndDate.Month == month))
            )
            .ToList();
    }

    public ICollection<int> GetTaskYearsByClassAndSubject(Class @class, Subject subject)
    {
        return _context.Set<Task>()
            .Where(t => t.ClassId == @class.Id && t.SubjectId == subject.Id)
            .Select(t => t.StartDate.Year).Distinct()
            .ToList();
    }

    public ICollection<int> GetTaskMonthByClassSubjectAndYear(Class @class, Subject subject, int year)
    {
        return _context.Set<Task>()
            .Where(t => t.ClassId == @class.Id && t.SubjectId == subject.Id && t.StartDate.Year == year)
            .Select(t => t.StartDate.Month).Distinct()
            .ToList();
    }
}