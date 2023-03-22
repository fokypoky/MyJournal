using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class TimetableRepository : EntityRepository<TimetableRepository>
{
    public TimetableRepository(DbContext context) : base(context)
    {
    }

    public ICollection<Timetable> GetTimetableByEmployee(Employee employee)
    {
        return _context.Set<Timetable>()
            .Where(t => t.TeacherId == employee.Id)
            .Include(t => t.Subject)
            .Include(t => t.Auditory)
            .ToList();
    }
}