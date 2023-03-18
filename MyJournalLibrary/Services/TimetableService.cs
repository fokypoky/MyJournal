using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournalLibrary.Services;

public class TimetableService : TimetableRepository
{
    public TimetableService(DbContext context) : base(context)
    {
    }
}