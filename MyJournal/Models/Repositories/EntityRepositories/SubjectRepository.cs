using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyJournal.Models.Repositories.Interfaces;
using MyJournalLibrary.Entities;

namespace MyJournal.Models.Repositories.EntityRepositories;

public class SubjectRepository : IEntityRepository<Subject>
{
    private readonly DbContext _dbContext;
    public SubjectRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Subject GetById(int id) => _dbContext.Set<Subject>().Find(id);

    public async void Add(Subject subject)
    {
        _dbContext.Add(subject);
        await _dbContext.SaveChangesAsync();
    }

    public async void AddRange(IEnumerable<Subject> subjects)
    {
        _dbContext.AddRange(subjects);
        await _dbContext.SaveChangesAsync();
    }

    public async void Delete(Subject subject)
    {
        _dbContext.Remove(subject);
        await _dbContext.SaveChangesAsync();
    }

    public async void DeleteRange(IEnumerable<Subject> subjects)
    {
        _dbContext.RemoveRange(subjects);
        await _dbContext.SaveChangesAsync();
    }

    public void Update(Subject entity)
    {
        throw new System.NotImplementedException();
    }
}