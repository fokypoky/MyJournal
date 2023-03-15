using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyJournal.Models.Repositories.Interfaces;
using MyJournalLibrary.Entities;

namespace MyJournal.Models.Repositories.EntityRepositories;

public class EmployeeRepository : IEntityRepository<Employee>
{
    private readonly DbContext _dbContext;

    public EmployeeRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Employee GetById(int id) => _dbContext.Set<Employee>().Find(id);

    public async void Add(Employee employee)
    {
        _dbContext.Add(employee);
        await _dbContext.SaveChangesAsync();
    }

    public async void AddRange(IEnumerable<Employee> employees)
    {
        _dbContext.AddRange(employees);
        await _dbContext.SaveChangesAsync();
    }

    public async void Delete(Employee employee)
    {
        _dbContext.Remove(employee);
        await _dbContext.SaveChangesAsync();
    }

    public async void DeleteRange(IEnumerable<Employee> employees)
    {
        _dbContext.RemoveRange(employees);
        await _dbContext.SaveChangesAsync();
    }

    public void Update(Employee entity)
    {
        throw new System.NotImplementedException();
    }

    public Employee GetByContacts(Contact contact) =>
        _dbContext.Set<Employee>().FirstOrDefault(e => e.ContactsId == contact.Id);
}