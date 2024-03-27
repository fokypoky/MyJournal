using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class ContactsRepository : EntityRepository<Contact>
{
    public ContactsRepository(DbContext context) : base(context)
    {
    }

    public string? GetPhoneNumberById(int id)
    {
	    return _context.Set<Contact>().FirstOrDefault(c => c.Id == id)?.PhoneNumber;
    }

    public string? GetEmailById(int id)
    {
	    return _context.Set<Contact>().FirstOrDefault(c => c.Id == id)?.Email;
    }

    //public override void Update(Contact contact)
    //{
	   // _context.Set<Contact>().AsNoTracking()
    //}

    public Contact? GetByLogin(string login, string password)
    {
        return _context.Set<Contact>()
            .Include(c => c.UserRole)
            .FirstOrDefault(c => (c.Email == login || c.PhoneNumber == login) && c.Password == password);
    }

    public Contact? GetByLoginAndRoleId(string login, int roleId)
    {
	    return _context.Set<Contact>()
		    .FirstOrDefault(c => c.UserRole.Id == roleId && (c.Email == login || c.PhoneNumber == login));
    }

    public bool IsPhoneNumberExists(string phoneNumber)
    {
	    return _context.Set<Contact>()
		    .FirstOrDefault(c => c.PhoneNumber == phoneNumber) is not null;
    }

    public bool IsEmailExists(string email)
    {
	    return _context.Set<Contact>()
		    .FirstOrDefault(c => c.Email == email) is not null;
    }

}