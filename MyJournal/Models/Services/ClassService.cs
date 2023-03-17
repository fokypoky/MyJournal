using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using MyJournal.Models.Repositories;
using MyJournal.Models.Repositories.EntityRepositories;
using MyJournal.Models.Repositories.Interfaces;
using MyJournalLibrary.Entities;

namespace MyJournal.Models.Services;

public class ClassService
{
    private readonly ClassRepository _repository;
    
    public ClassService(DbContext context)
    {
        _repository = new ClassRepository(context);
    }

    public Class GetById(int id) => _repository.GetById(id);
    public void Add(Class _class) => _repository.Add(_class);
    public void AddRange(IEnumerable<Class> classes) => _repository.AddRange(classes);
    public void Delete(Class _class) => _repository.Delete(_class);
    public void DeleteRange(IEnumerable<Class> classes) => _repository.DeleteRange(classes);
    public void Update(Class _class) => _repository.Update(_class);
    public ICollection<Class> GetClassesByEmployee(Employee employee) => _repository.GetClassesByEmployee(employee);

}