using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournalLibrary.Services;

public class ContactsService : ContactsRepository
{
    public ContactsService(DbContext context) : base(context)
    {
    }
}