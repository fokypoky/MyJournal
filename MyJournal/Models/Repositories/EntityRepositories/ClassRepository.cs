using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyJournal.Models.Repositories.Interfaces;
using MyJournalLibrary.Entities;

namespace MyJournal.Models.Repositories.EntityRepositories;

public class ClassRepository : IEntityRepository<Class>
{
    private readonly DbContext _dbContext;

    public ClassRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Class GetById(int id) => _dbContext.Set<Class>().Find(id);

    public async void Add(Class _class)
    {
        _dbContext.Add(_class);
        await _dbContext.SaveChangesAsync();
    }

    public async void AddRange(IEnumerable<Class> classes)
    {
        _dbContext.AddRange(classes);
        await _dbContext.SaveChangesAsync();
    }

    public async void Delete(Class _class)
    {
        _dbContext.Remove(_class);
        await _dbContext.SaveChangesAsync();
    }

    public async void DeleteRange(IEnumerable<Class> classes)
    {
        _dbContext.RemoveRange(classes);
        await _dbContext.SaveChangesAsync();
    }

    public void Update(Class entity)
    {
        throw new System.NotImplementedException();
    }

    public ICollection<Class> GetClassesByEmployee(Employee employee)
    {
        _dbContext.Set<Class>().Where(c => c.LeaderId == employee.Id).Load();
        return employee.Classes;
    }
}