using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyJournal.Models.Repositories.Interfaces;
using MyJournalLibrary.Entities;

namespace MyJournal.Models.Repositories.EntityRepositories;

public class MarkRepository : IEntityRepository<Mark>
{
    private readonly DbContext _dbContext;

    public MarkRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Mark GetById(int id) => _dbContext.Set<Mark>().Find(id);

    public async void Add(Mark mark)
    {
        _dbContext.Add(mark);
        await _dbContext.SaveChangesAsync();
    }

    public async void AddRange(IEnumerable<Mark> marks)
    {
        _dbContext.AddRange(marks);
        await _dbContext.SaveChangesAsync();
    }

    public async void Delete(Mark mark)
    {
        _dbContext.Remove(mark);
        await _dbContext.SaveChangesAsync();
    }

    public async void DeleteRange(IEnumerable<Mark> marks)
    {
        _dbContext.RemoveRange(marks);
        await _dbContext.SaveChangesAsync();
    }

    public void Update(Mark entity)
    {
        throw new System.NotImplementedException();
    }
}