using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyJournal.Models.Repositories.Interfaces;

namespace MyJournal.Models.Repositories;

public class EntityRepository<T> : IEntityRepository<T> where T : class
{
    private readonly DbContext _dbContext;

    public EntityRepository(DbContext context)
    {
        _dbContext = context;
    }

    public T GetById(int id) => _dbContext.Set<T>().Find(id);

    public async void Add(T entity)
    {
        _dbContext.Add(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async void AddRange(IEnumerable<T> entities)
    { 
        _dbContext.AddRange(entities);
        await _dbContext.SaveChangesAsync();
    }

    public async void Delete(T entity)
    {
        _dbContext.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async void DeleteRange(IEnumerable<T> entities)
    {
        _dbContext.RemoveRange(entities);
        await _dbContext.SaveChangesAsync();
    }

    public void Update(T entity)
    {
        
    }
}