using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class ParentsRepository : EntityRepository<Parent>
{
    public ParentsRepository(DbContext context) : base(context)
    {
    }

    public Parent? GetByUserId(int userId)
    {
	    return _context.Set<Parent>().Where(p => p.ContactsId == userId)?.FirstOrDefault();
    }
}