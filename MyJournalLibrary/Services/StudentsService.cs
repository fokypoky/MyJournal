using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournalLibrary.Services;

public class StudentsService : StudentsRepository
{
    public StudentsService(DbContext context) : base(context)
    {
    }
}