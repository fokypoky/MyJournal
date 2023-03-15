using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyJournal.Models.Repositories.Interfaces;
using MyJournalLibrary.Entities;

namespace MyJournal.Models.Repositories.EntityRepositories;

public class TaskRepository : IEntityRepository<Task>
{
    private readonly DbContext _dbContext;
    public TaskRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task GetById(int id) => _dbContext.Set<Task>().Find(id);

    public async void Add(Task task)
    {
        _dbContext.Add(task);
        await _dbContext.SaveChangesAsync();
    }

    public async void AddRange(IEnumerable<Task> tasks)
    {
        _dbContext.AddRange(tasks);
        await _dbContext.SaveChangesAsync();
    }

    public async void Delete(Task task)
    {
        _dbContext.Remove(task);
        await _dbContext.SaveChangesAsync();
    }

    public async void DeleteRange(IEnumerable<Task> tasks)
    {
        _dbContext.RemoveRange(tasks);
        await _dbContext.SaveChangesAsync();
    }

    public void Update(Task entity)
    {
        throw new System.NotImplementedException();
    }
}