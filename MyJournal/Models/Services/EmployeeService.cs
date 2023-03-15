using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyJournal.Models.Repositories;
using MyJournal.Models.Repositories.EntityRepositories;
using MyJournal.Models.Repositories.Interfaces;
using MyJournalLibrary.Entities;

namespace MyJournal.Models.Services;

public class EmployeeService
{
    private readonly EmployeeRepository _repository;

    public EmployeeService(DbContext context)
    {
        _repository = new EmployeeRepository(context);
    }

    public Employee GetById(int id) => _repository.GetById(id);
    public void Add(Employee employee) => _repository.Add(employee);
    public void AddRande(IEnumerable<Employee> employees) => _repository.AddRange(employees);
    public void Delete(Employee employee) => _repository.Delete(employee);
    public void DeleteRange(IEnumerable<Employee> employees) => _repository.DeleteRange(employees);
    public void Update(Employee employee) => _repository.Update(employee);
    public Employee GetByContacts(Contact contact) => _repository.GetByContacts(contact);
}