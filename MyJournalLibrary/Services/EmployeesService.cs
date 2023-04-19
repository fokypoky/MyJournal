using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournalLibrary.Services;

public class EmployeesService : EmployeesRepository
{
    public EmployeesService(DbContext context) : base(context)
    {
    }
}