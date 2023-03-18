using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class ParentsRepository : EntityRepository<Parent>
{
    public ParentsRepository(DbContext context) : base(context)
    {
    }
}