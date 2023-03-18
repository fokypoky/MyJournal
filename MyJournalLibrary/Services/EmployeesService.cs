using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournalLibrary.Services;

public class EmployeesService : EmployeesRepository
{
    public EmployeesService(DbContext context) : base(context)
    {
    }
}