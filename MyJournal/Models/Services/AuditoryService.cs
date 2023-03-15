using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyJournal.Models.Repositories;
using MyJournal.Models.Repositories.EntityRepositories;
using MyJournal.Models.Repositories.Interfaces;
using MyJournalLibrary.Entities;

namespace MyJournal.Models.Services;

public class AuditoryService
{
    private readonly AuditoryRepository _repository;

    public AuditoryService(DbContext context)
    {
        _repository = new AuditoryRepository(context);
    }

    public Auditory GetById(int id) => _repository.GetById(id);
    public void Add(Auditory auditory) => _repository.Add(auditory);
    public void AddRange(IEnumerable<Auditory> auditories) => _repository.AddRange(auditories);
    public void Delete(Auditory auditory) => _repository.Delete(auditory);
    public void DeleteRange(IEnumerable<Auditory> auditories) => _repository.DeleteRange(auditories);
    public void Update(Auditory auditory) => _repository.Update(auditory);
}