using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class ContactsRepository : EntityRepository<Contact>
{
    public ContactsRepository(DbContext context) : base(context)
    {
    }
}