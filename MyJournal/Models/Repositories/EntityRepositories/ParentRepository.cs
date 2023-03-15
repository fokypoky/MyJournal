using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyJournal.Models.Repositories.Interfaces;
using MyJournalLibrary.Entities;

namespace MyJournal.Models.Repositories.EntityRepositories;

public class ParentRepository : IEntityRepository<Parent>
{
    private readonly DbContext _dbContext;
    public ParentRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Parent GetById(int id) => _dbContext.Set<Parent>().Find(id);

    public async void Add(Parent parent)
    {
        _dbContext.Add(parent);
        await _dbContext.SaveChangesAsync();
    }

    public async void AddRange(IEnumerable<Parent> parents)
    {
        _dbContext.AddRange(parents);
        await _dbContext.SaveChangesAsync();
    }

    public async void Delete(Parent parent)
    {
        _dbContext.Remove(parent);
        await _dbContext.SaveChangesAsync();
    }

    public async void DeleteRange(IEnumerable<Parent> parents)
    {
        _dbContext.RemoveRange(parents);
        await _dbContext.SaveChangesAsync();
    }

    public void Update(Parent entity)
    {
        throw new System.NotImplementedException();
    }
}