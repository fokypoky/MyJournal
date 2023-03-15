using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyJournal.Models.Repositories.Interfaces;
using MyJournalLibrary.Entities;

namespace MyJournal.Models.Repositories.EntityRepositories;

public class StudentRepository : IEntityRepository<Student>
{
    private readonly DbContext _dbContext;

    public StudentRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Student GetById(int id) => _dbContext.Set<Student>().Find(id);

    public async void Add(Student student)
    {
        _dbContext.Add(student);
        await _dbContext.SaveChangesAsync();
    }

    public async void AddRange(IEnumerable<Student> students)
    {
        _dbContext.AddRange(students);
        await _dbContext.SaveChangesAsync();
    }

    public async void Delete(Student student)
    {
        _dbContext.Remove(student);
        await _dbContext.SaveChangesAsync();
    }

    public async void DeleteRange(IEnumerable<Student> students)
    {
        _dbContext.RemoveRange(students);
        await _dbContext.SaveChangesAsync();
    }

    public void Update(Student entity)
    {
        throw new System.NotImplementedException();
    }
}