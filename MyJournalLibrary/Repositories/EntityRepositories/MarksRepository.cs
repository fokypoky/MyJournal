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
            .Include(m => m.Student)
                .ThenInclude(s => s.Class)
            .Where(m => m.Student.Class == @class && m.Subject == subject)
            .ToList();
    }
    
}