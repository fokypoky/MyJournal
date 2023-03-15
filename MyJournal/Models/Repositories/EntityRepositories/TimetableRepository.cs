using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyJournal.Models.Repositories.Interfaces;
using MyJournalLibrary.Entities;

namespace MyJournal.Models.Repositories.EntityRepositories;

public class TimetableRepository : IEntityRepository<Timetable>
{
    private readonly DbContext _dbContext;
    public TimetableRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Timetable GetById(int id) => _dbContext.Set<Timetable>().Find(id);

    public async void Add(Timetable timetable)
    {
        _dbContext.Add(timetable);
        await _dbContext.SaveChangesAsync();
    }

    public async void AddRange(IEnumerable<Timetable> timetables)
    {
        _dbContext.AddRange(timetables);
        await _dbContext.SaveChangesAsync();
    }

    public async void Delete(Timetable timetable)
    {
        _dbContext.Remove(timetable);
        await _dbContext.SaveChangesAsync();
    }

    public async void DeleteRange(IEnumerable<Timetable> timetables)
    {
        _dbContext.RemoveRange(timetables);
        await _dbContext.SaveChangesAsync();
    }

    public void Update(Timetable entity)
    {
        throw new System.NotImplementedException();
    }
}