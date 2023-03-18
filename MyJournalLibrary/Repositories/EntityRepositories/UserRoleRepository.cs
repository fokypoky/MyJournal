using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class UserRoleRepository : EntityRepository<UserRole>
{
    public UserRoleRepository(DbContext context) : base(context)
    {
    }
}