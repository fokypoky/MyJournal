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
            .Include(m => m.Task)
            .Include(m => m.Student)
                .ThenInclude(s => s.Class)
            .Include(m => m.Student.Contacts)
            .Where(m => m.Student.Class == @class && m.Subject == subject)
            .ToList();
    }

    public override void AddRange(IEnumerable<Mark> marks)
    {
        //foreach (var mark in marks)
        //{
        //    _context.Set<Mark>().Add(mark);
        //}
        _context.Set<Mark>().AddRange(marks);
        _context.SaveChanges();
    }
}