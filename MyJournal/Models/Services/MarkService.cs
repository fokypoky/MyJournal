using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyJournal.Models.Repositories;
using MyJournal.Models.Repositories.EntityRepositories;
using MyJournal.Models.Repositories.Interfaces;
using MyJournalLibrary.Entities;

namespace MyJournal.Models.Services;

public class MarkService
{
    private readonly MarkRepository _repository;

    public MarkService(DbContext context)
    {
        _repository = new MarkRepository(context);
    }

    public Mark GetById(int id) => _repository.GetById(id);
    public void Add(Mark mark) => _repository.Add(mark);
    public void AddRange(IEnumerable<Mark> marks) => _repository.AddRange(marks);
    public void Delete(Mark mark) => _repository.Delete(mark);
    public void DeleteRange(IEnumerable<Mark> marks) => _repository.DeleteRange(marks);
    public void Update(Mark mark) => _repository.Update(mark);
}