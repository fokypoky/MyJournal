using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class MarksRepository : EntityRepository<Mark>
{
    public MarksRepository(DbContext context) : base(context)
    {
    }
}