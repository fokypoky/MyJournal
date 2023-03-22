using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class EmployeesRepository : EntityRepository<Employee>
{
    public EmployeesRepository(DbContext context) : base(context)
    {
    }

    public Employee? GetByContactId(int id)
    {
        return _context.Set<Employee>()
            .Include(e => e.Subjects)
            .Include(e => e.Classes)
            .Include(e => e.Contacts)
            .FirstOrDefault(e => e.ContactsId == id);
    }
}