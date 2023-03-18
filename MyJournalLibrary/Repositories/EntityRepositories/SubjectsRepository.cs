using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class SubjectsRepository : EntityRepository<Subject>
{
    public SubjectsRepository(DbContext context) : base(context)
    {
    }
}