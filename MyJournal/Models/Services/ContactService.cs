using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyJournal.Models.Repositories;
using MyJournal.Models.Repositories.Interfaces;
using MyJournalLibrary.Entities;

namespace MyJournal.Models.Services;

public class ContactService
{
    private IEntityRepository<Contact> _repository;

    public ContactService(DbContext context)
    {
        _repository = new EntityRepository<Contact>(context);
    }

    public Contact GetById(int id) => _repository.GetById(id);
    public void Add(Contact contact) => _repository.Add(contact);
    public void AddRange(IEnumerable<Contact> contacts) => _repository.AddRange(contacts);
    public void Delete(Contact contact) => _repository.Delete(contact);
    public void DeleteRange(IEnumerable<Contact> contacts) => _repository.DeleteRange(contacts);
    public void Update(Contact contact) => _repository.Update(contact);
}