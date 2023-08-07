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

    public ICollection<Timetable> GetByStudent(Student student)
    {
        return _context.Set<Timetable>()
            .Where(t => t.ClassId == student.ClassId)
            .Include(t => t.Subject)
            .Include(t => t.Auditory)
            .Include(t => t.Teacher)
                .ThenInclude(t => t.Contacts)
            .ToList();
    }
}