using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class StudentsRepository : EntityRepository<Student>
{
    public StudentsRepository(DbContext context) : base(context)
    {
    }
    public ICollection<Student> GetByClass(Class @class)
    {
        return _context.Set<Student>()
            .Include(s => s.Contacts)
            .Include(s => s.Marks)
                .ThenInclude(m => m.Task)
            .Where(s => s.ClassId == @class.Id)
            .ToList();
    }
}