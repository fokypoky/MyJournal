using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class ClassRepository : EntityRepository<Class>
{
    public ClassRepository(DbContext context) : base(context)
    {
    }
}