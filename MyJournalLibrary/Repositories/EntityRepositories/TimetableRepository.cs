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

    public ICollection<Timetable> GetByClassWithEmployeeContactsSubjectAndAuditory(Class @class)
    {
	    return _context.Set<Timetable>()
		    .Include(t => t.Auditory)
		    .Include(t => t.Subject)
		    .Include(t => t.Teacher)
				.ThenInclude(e => e.Contacts)
		    .Where(t => t.ClassId == @class.Id)
		    .ToList();
    }

    public void Remove(Timetable timetable)
    {
	    _context.Set<Timetable>().Remove(timetable);
	    _context.SaveChanges();
    }
}