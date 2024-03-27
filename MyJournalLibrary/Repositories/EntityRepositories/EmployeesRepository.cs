using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class EmployeesRepository : EntityRepository<Employee>
{
    public EmployeesRepository(DbContext context) : base(context)
    {
    }

    /// <summary>
    /// Удаляет связку "Сотрудник-предмет"
    /// </summary>
    /// <param name="employee"></param>
    public void RemoveSubjects(Employee employee)
    {
        _context.Set<EmployeeSubject>().RemoveRange(
	        _context.Set<EmployeeSubject>()
		           .Where(es => es.EmployeeId == employee.Id)
	        );
    }

    public ICollection<Employee> GetFreeClassEmployeesWithContacts()
    {
	    return _context.Set<Employee>()
		    .Where(e => !_context.Set<Class>()
			    .Where(c => c.Leader != null)
			    .Select(c => c.Leader)
			    .Contains(e))
		    .Include(e => e.Contacts)
		    .ToList();
    }

    public void AddSubjectToEmployee(Employee employee, Subject subject)
    {
	    _context.Set<EmployeeSubject>()
		    .Add(new EmployeeSubject() { EmployeeId = employee.Id, SubjectId = subject.Id });
        _context.SaveChanges();
    }

    public void RemoveSubjectFromEmployee(Employee employee, Subject subject)
    {
        var employeeSubject = _context.Set<EmployeeSubject>()
	        .Where(es => es.EmployeeId == employee.Id && es.SubjectId == subject.Id)
	        .FirstOrDefault();

        if (employeeSubject != null)
        {
            _context.Set<EmployeeSubject>().Remove(employeeSubject);
            _context.SaveChanges();
        }
    }

    public int? GetIdByPhoneNumber(string phoneNumber)
    {
        return _context.Set<Employee>()
	        .Where(e => e.Contacts.PhoneNumber == phoneNumber)
	        .Select(e => e.Id)
	        .FirstOrDefault();
    }

    public void AddSubjectsToEmployee(Employee employee, IEnumerable<Subject> subjects)
    {
	    var employeeSubjects = new List<EmployeeSubject>();
	    foreach (var subject in subjects)
	    {
            employeeSubjects.Add(new EmployeeSubject() { EmployeeId = employee.Id, SubjectId = subject.Id});
	    }

	    _context.Set<EmployeeSubject>().AddRange(employeeSubjects);
        _context.SaveChanges();
    }


    public Employee? GetByContactId(int id)
    {
        return _context.Set<Employee>()
            .Include(e => e.Subjects)
            .Include(e => e.Classes)
            .Include(e => e.Contacts)
            .FirstOrDefault(e => e.ContactsId == id);
    }

    public ICollection<Employee> GetAllWithContacts()
    {
	    return _context.Set<Employee>()
		    .Include(e => e.Contacts)
		    .ToList();
    }

    public ICollection<Employee> GetNotInRange(List<Employee> employees)
    {
        return _context.Set<Employee>()
	        .Where(e => !employees.Contains(e))
	        .ToList();
    }
}