using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class TimetableRepository : EntityRepository<Timetable>
{
    public TimetableRepository(DbContext context) : base(context)
    {
    }

    public void UpdateRange(List<Timetable> timetables)
    {
        _context.Set<Timetable>().UpdateRange(timetables);
        _context.SaveChanges();
    }

    public ICollection<Timetable> GetByEmployee(Employee employee)
    {
        return _context.Set<Timetable>()
	        .Where(t => t.TeacherId == employee.Id)
	        .ToList();
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

    public bool IsClassTimeFree(Class @class, int dayOfWeek, TimeOnly time)
    {
	    return _context.Set<Timetable>()
		    .FirstOrDefault(t => t.ClassId == @class.Id
		                         && t.DayOfWeek == dayOfWeek
		                         && t.LessonTime == time) is null;
    }
}