using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyJournal.Models.Repositories;
using MyJournal.Models.Repositories.EntityRepositories;
using MyJournal.Models.Repositories.Interfaces;
using MyJournalLibrary.Entities;

namespace MyJournal.Models.Services;

public class TaskService
{
    private readonly TaskRepository _repository;

    public TaskService(DbContext context)
    {
        _repository = new TaskRepository(context);
    }

    public Task GetById(int id) => _repository.GetById(id);
    public void Add(Task task) => _repository.Add(task);
    public void AddRange(IEnumerable<Task> tasks) => _repository.AddRange(tasks);
    public void Delete(Task task) => _repository.Delete(task);
    public void DeleteRange(IEnumerable<Task> tasks) => _repository.DeleteRange(tasks);
    public void Update(Task task) => _repository.Update(task);
}