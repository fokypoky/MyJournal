using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;
using Task = MyJournalLibrary.Entities.Task;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class TasksRepository : EntityRepository<Task>
{
    public TasksRepository(DbContext context) : base(context)
    {
    }

    public void RemoveAllBySubject(Subject subject)
    {
        _context.Set<Task>()
	        .RemoveRange(_context.Set<Task>().Where(t => t.SubjectId == subject.Id));
        _context.SaveChanges();
    }

    public ICollection<Task> GetByEmployee(Employee employee)
    {
        return _context.Set<Task>()
	        .Where(t => t.TeacherId == employee.Id)
	        .ToList();
    }

    public void UpdateRange(List<Task> tasks)
    {
        _context.Set<Task>().UpdateRange(tasks);
        _context.SaveChanges();
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

    public ICollection<Task> GetByStudentSubjectAndPeriod(Student student, Subject subject, int year, int month)
    {
		return _context.Set<Task>()
			.Where(t => t.ClassId == student.ClassId && t.SubjectId == subject.Id
			                                   && ((t.StartDate.Year == year && t.StartDate.Month == month) ||
			                                       (t.EndDate.Year == year && t.EndDate.Month == month))
			)
			.ToList();
	}

    public ICollection<Task> GetByClassSubjectAndStartDatePeriod(Class @class, Subject subject, int year, int month)
    {
        return _context.Set<Task>()
            .Where(t => t.ClassId == @class.Id && t.SubjectId == subject.Id && (t.StartDate.Year == year && t.StartDate.Month == month))
            .ToList();
    }

    public ICollection<int> GetTaskYearsByClassAndSubject(Class @class, Subject subject)
    {
        return _context.Set<Task>()
            .Where(t => t.ClassId == @class.Id && t.SubjectId == subject.Id)
            .Select(t => t.StartDate.Year).Distinct()
            .ToList();
    }

    public ICollection<int> GetTaskYearsByStudentAndSubject(Student student, Subject subject)
    {
        return _context.Set<Task>()
	        .Where(t => t.ClassId == student.ClassId && t.SubjectId == subject.Id)
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

    public ICollection<int> GetTaskMonthByStudentSubjectAndYear(Student student, Subject subject, int year)
    {
	    return _context.Set<Task>()
		    .Where(t => t.ClassId == student.ClassId && t.SubjectId == subject.Id && t.StartDate.Year == year)
		    .Select(t => t.StartDate.Month).Distinct()
		    .ToList();
    }

    public ICollection<Task> GetExpiringTasksByClassSubjectAndDate(Class @class, Subject subject, DateTime currentDate)
    {
        return _context.Set<Task>()
            .Where(t => t.ClassId == @class.Id && t.SubjectId == subject.Id
                                               && (currentDate.DayOfYear - t.EndDate.DayOfYear <= 7)
            ).ToList();
    }

    public ICollection<Task> GetExpiringTasksByStudentSubjectAndDate(Student student, Subject subject,
	    DateTime currentDate)
    {
	    return _context.Set<Task>()
		    .Where(t => t.ClassId == student.ClassId && t.SubjectId == subject.Id
		                                       && (currentDate.DayOfYear - t.EndDate.DayOfYear <= 7)
		    ).ToList();
	}
}