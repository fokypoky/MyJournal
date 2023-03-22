using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class UserRoleRepository : EntityRepository<UserRole>
{
    public UserRoleRepository(DbContext context) : base(context)
    {
    }
    public UserRole GetByLogin(string login, string password)
    {
        var contact = _context.Set<Contact>()
            .Include(c => c.UserRole)
            .FirstOrDefault(c => (c.Email == login || c.PhoneNumber == login) && c.Password == password);
        return contact?.UserRole;
    } 
}