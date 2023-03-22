using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

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
        UserRoleService service = new UserRoleService(_context);
        return service.GetByLogin(login: login, password: password);
    }

    
}