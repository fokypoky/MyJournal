using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyJournal.Models.Repositories;
using MyJournal.Models.Repositories.EntityRepositories;
using MyJournal.Models.Repositories.Interfaces;
using MyJournalLibrary.Entities;

namespace MyJournal.Models.Services;

public class SubjectService
{
    private readonly SubjectRepository _repository;

    public SubjectService(DbContext context)
    {
        _repository = new SubjectRepository(context);
    }

    public Subject GetById(int id) => _repository.GetById(id);
    public void Add(Subject subject) => _repository.Add(subject);
    public void AddRange(IEnumerable<Subject> subjects) => _repository.AddRange(subjects);
    public void Delete(Subject subject) => _repository.Delete(subject);
    public void DeleteRange(IEnumerable<Subject> subjects) => _repository.DeleteRange(subjects);
    public void Update(Subject subject) => _repository.Update(subject);
}