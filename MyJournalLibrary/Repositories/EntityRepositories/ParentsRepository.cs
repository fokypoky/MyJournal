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

    public Parent? GetByUserIdWithContactsStudentsAndStudentContacts(int userId)
    {
	    return _context.Set<Parent>()
		    .Where(p => p.ContactsId == userId)
		    .Include(p => p.Contacts)
		    .Include(p => p.Students)
				.ThenInclude(s => s.Contacts)
            .Include(p => p.Students)
				.ThenInclude(s => s.Class)
		    .FirstOrDefault();
    }

    public Parent? GetByContacts(Contact contacts)
    {
	    return _context.Set<Parent>()
		    .FirstOrDefault(p => p.ContactsId == contacts.Id);
    }
}