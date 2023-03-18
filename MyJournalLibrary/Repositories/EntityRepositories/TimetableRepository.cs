using Microsoft.EntityFrameworkCore;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class TimetableRepository : EntityRepository<TimetableRepository>
{
    public TimetableRepository(DbContext context) : base(context)
    {
    }
}