using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class EmployeesRepository : EntityRepository<Employee>
{
    public EmployeesRepository(DbContext context) : base(context)
    {
    }
}