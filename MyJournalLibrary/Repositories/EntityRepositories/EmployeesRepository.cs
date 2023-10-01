﻿using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class EmployeesRepository : EntityRepository<Employee>
{
    public EmployeesRepository(DbContext context) : base(context)
    {
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
}