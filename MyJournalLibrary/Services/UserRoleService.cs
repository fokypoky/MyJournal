using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournalLibrary.Services;

public class UserRoleService : UserRoleRepository
{
    public UserRoleService(DbContext context) : base(context)
    {
    }
    
}