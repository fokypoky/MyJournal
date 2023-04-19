using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class ClassRepository : EntityRepository<Class>
{
    public ClassRepository(DbContext context) : base(context)
    {
    }
    public ICollection<Class> GetByEmployee(Employee employee)
    {
        return _context.Set<Class>()
            .Where(c => c.Leader.Id == employee.Id)
            .ToList();
    } 
}