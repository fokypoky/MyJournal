using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournalLibrary.Services;

public class LoginService
{
    private readonly DbContext _context;
    public LoginService(DbContext context)
    {
        _context = context;
    }
    public UserRole GetUserRoleByLogin(string login, string password)
    {
        UserRoleRepository service = new UserRoleRepository(_context);
        return service.GetByLogin(login: login, password: password);
    }

    
}