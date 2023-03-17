using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyJournal.Models.Repositories.Interfaces;
using MyJournalLibrary.Entities;

namespace MyJournal.Models.Repositories.EntityRepositories;

public class AuditoryRepository : IEntityRepository<Auditory>
{
    private readonly DbContext _dbContext;

    public AuditoryRepository(DbContext context)
    {
        _dbContext = context;
    }

    public Auditory GetById(int id) => _dbContext.Set<Auditory>().Find(id);

    public async void Add(Auditory auditory)
    {
        _dbContext.Add(auditory);
        await _dbContext.SaveChangesAsync();
    }

    public async void AddRange(IEnumerable<Auditory> auditories)
    {
        _dbContext.AddRange(auditories);
        await _dbContext.SaveChangesAsync();
    }

    public async void Delete(Auditory auditory)
    {
        _dbContext.Remove(auditory);
        await _dbContext.SaveChangesAsync();
    }

    public async void DeleteRange(IEnumerable<Auditory> entities)
    {
        _dbContext.RemoveRange(entities);
        await _dbContext.SaveChangesAsync();
    }

    public void Update(Auditory entity)
    {
        throw new System.NotImplementedException();
    }
}