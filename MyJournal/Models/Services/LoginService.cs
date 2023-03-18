using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MyJournal.Models.Services;

public class LoginService
{
    private string _login;
    private string _password;
    
    public LoginService(string login, string password)
    {
        _login = login;
        _password = password;
    }

    public void FillApplicationData()
    {
        // TODO: переписать
        /*using (var db = new ApplicationContext())
        {
            var contact = db.Contacts.FirstOrDefault(c =>
                (c.PhoneNumber == _login || c.Email == _login) && c.Password == _password);
            if (contact != null)
            {
                db.Parents.Where(p => p.ContactsId == contact.Id).Load();
                db.Employees.Where(e => e.ContactsId == contact.Id).Load();
                db.Students.Where(s => s.ContactsId == contact.Id).Load();

                if (contact.Parents?.Count > 0)
                {
                    ApplicationData.UserRole = UserRole.Parent;
                    ApplicationData.UserId = contact.Id;
                }
                else if (contact.Students?.Count > 0)
                {
                    ApplicationData.UserRole = UserRole.Student;
                    ApplicationData.UserId = contact.Id;
                }
                else if (contact.Employees?.Count > 0)
                {
                    ApplicationData.UserRole = UserRole.Employee;
                    ApplicationData.UserId = contact.Id;
                }
                else
                {
                    ApplicationData.UserRole = UserRole.None;
                    ApplicationData.UserId = 0;
                }
            }
            else
            {
                ApplicationData.UserRole = UserRole.None;
            }
        }*/
    }
}