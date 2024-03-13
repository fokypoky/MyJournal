using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class MarksRepository : EntityRepository<Mark>
{
    public MarksRepository(DbContext context) : base(context)
    {
    }

    public void RemoveByEmployee(Employee employee)
    {
	    _context.Set<Mark>()
		    .RemoveRange(_context.Set<Mark>()
				    .Where(m => m.TeacherId == employee.Id));
    }

    public ICollection<Mark> GetWithTeacherAndContactsByStudentSubjectAndPeriod(Student student, Subject subject,
        int year, int month)
    {
        return _context.Set<Mark>()
            .Where(m => m.StudentId == student.Id && m.SubjectId == subject.Id && m.MarkDate.Year == year && m.MarkDate.Month == month)
            .Include(m => m.Teacher)
                .ThenInclude(t => t.Contacts)
            .ToList();
    }
    public ICollection<int> GetMarkYearsByStudentAndSubject(Student student, Subject subject)
    {
        return _context.Set<Mark>()
            .Where(m => m.StudentId == student.Id && m.SubjectId == subject.Id)
            .Select(m => m.MarkDate.Year).Distinct()
            .ToList();
    }

    public ICollection<int> GetMarkMonthsByStudentSubjectAndYear(Student student, Subject subject, int year)
    {
        return _context.Set<Mark>()
            .Where(m => m.StudentId == student.Id && m.SubjectId == subject.Id && m.MarkDate.Year == year)
            .Select(m => m.MarkDate.Month).Distinct()
            .ToList();
    }
    public ICollection<Mark> GetMarksByClassAndSubject(Class @class, Subject subject)
    {
        return _context.Set<Mark>()
            .Include(m => m.Subject)
            //.Include(m => m.Task)
            .Include(m => m.Student)
                .ThenInclude(s => s.Class)
            .Include(m => m.Student.Contacts)
            .Where(m => m.Student.Class == @class && m.Subject == subject)
            .ToList();
    }

    public Mark? GetByStudentSubjectAndDate(Student student, Subject subject, DateTime date)
    {
        return _context.Set<Mark>()
            .Where(m => m.StudentId == student.Id && m.SubjectId == subject.Id
            && m.MarkDate.Date == date.Date)
            .FirstOrDefault();
    }

    public ICollection<Mark> GetByStudentAndSubject(Student student, Subject subject)
    {
        return _context.Set<Mark>()
            .Where(m => m.StudentId == student.Id && m.SubjectId == subject.Id)
            .ToList();
    }

    public ICollection<Mark> GetByStudent(Student student)
    {
        return _context.Set<Mark>()
	        .Where(m => m.StudentId == student.Id)
	        .ToList();
    }
    
    public override void UpdateRange(IEnumerable<Mark> marks)
    {
        foreach (var mark in marks)
        {
            _context.Set<Mark>().Attach(mark);
            _context.Entry(mark).State = EntityState.Modified;
        }

        _context.SaveChanges();
    }

    public override void AddRange(List<Mark> marks)
    {
        _context.Set<Mark>().AddRange(marks);
    }
}