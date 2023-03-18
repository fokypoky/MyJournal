using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class AuditoriesRepository : EntityRepository<Auditory>
{
    public AuditoriesRepository(DbContext context) : base(context)
    {
    }
}