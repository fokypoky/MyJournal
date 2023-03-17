using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyJournal.Models.Repositories;
using MyJournal.Models.Repositories.EntityRepositories;
using MyJournal.Models.Repositories.Interfaces;
using MyJournalLibrary.Entities;

namespace MyJournal.Models.Services;

public class StudentService
{
    private readonly StudentRepository _repository;

    public StudentService(DbContext context)
    {
        _repository = new StudentRepository(context);
    }

    public Student GetById(int id) => _repository.GetById(id);
    public void Add(Student student) => _repository.Add(student);
    public void AddRange(IEnumerable<Student> students) => _repository.AddRange(students);
    public void Delete(Student student) => _repository.Delete(student);
    public void DeleteRange(IEnumerable<Student> students) => _repository.DeleteRange(students);
    public void Update(Student student) => _repository.Update(student);
}