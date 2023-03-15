using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyJournal.Models.Repositories;
using MyJournal.Models.Repositories.Interfaces;
using MyJournalLibrary.Entities;

namespace MyJournal.Models.Services;

public class TimetableService
{
    private readonly IEntityRepository<Timetable> _repository;

    public TimetableService(DbContext context)
    {
        _repository = new EntityRepository<Timetable>(context);
    }

    public Timetable GetById(int id) => _repository.GetById(id);
    public void Add(Timetable timetable) => _repository.Add(timetable);
    public void AddRange(IEnumerable<Timetable> timetables) => _repository.AddRange(timetables);
    public void Delete(Timetable timetable) => _repository.Delete(timetable);
    public void DeleteRange(IEnumerable<Timetable> timetables) => _repository.DeleteRange(timetables);
    public void Update(Timetable timetable) => _repository.Update(timetable);
}