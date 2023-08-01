using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class MarksRepository : EntityRepository<Mark>
{
    public MarksRepository(DbContext context) : base(context)
    {
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

    public override void UpdateRange(IEnumerable<Mark> marks)
    {
        foreach (var mark in marks)
        {
            _context.Set<Mark>().Attach(mark);
            _context.Entry(mark).State = EntityState.Modified;
        }

        _context.SaveChanges();
    }

    public override void AddRange(IEnumerable<Mark> marks)
    {
        _context.Set<Mark>().AddRange(marks);
        _context.SaveChanges();
    }
}