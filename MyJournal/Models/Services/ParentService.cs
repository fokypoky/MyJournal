using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyJournal.Models.Repositories;
using MyJournal.Models.Repositories.Interfaces;
using MyJournalLibrary.Entities;

namespace MyJournal.Models.Services;

public class ParentService
{
    private readonly IEntityRepository<Parent> _repository;

    public ParentService(DbContext context)
    {
        _repository = new EntityRepository<Parent>(context);
    }

    public Parent GetById(int id) => _repository.GetById(id);
    public void Add(Parent parent) => _repository.Add(parent);
    public void AddRange(IEnumerable<Parent> parents) => _repository.AddRange(parents);
    public void Delete(Parent parent) => _repository.Delete(parent);
    public void DeleteRange(IEnumerable<Parent> parents) => _repository.DeleteRange(parents);
    public void Update(Parent parent) => _repository.Update(parent);
}