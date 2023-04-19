using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class SubjectsRepository : EntityRepository<Subject>
{
    public SubjectsRepository(DbContext context) : base(context)
    {
    }

    public ICollection<Subject> GetWithClassesByEmployee(Employee employee)
    {
        return _context
            .Set<Subject>()
            .Include(s => s.Classes)
            .Where(s => s.Employees.Contains(employee)).ToList();
    }
}