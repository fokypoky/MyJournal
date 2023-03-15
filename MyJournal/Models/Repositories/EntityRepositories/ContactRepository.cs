using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyJournal.Models.Repositories.Interfaces;
using MyJournalLibrary.Entities;

namespace MyJournal.Models.Repositories.EntityRepositories;

public class ContactRepository : IEntityRepository<Contact>
{
    private readonly DbContext _dbContext;

    public ContactRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Contact GetById(int id) => _dbContext.Set<Contact>().Find(id);

    public async void Add(Contact contact)
    {
        _dbContext.AddRange(contact);
        await _dbContext.SaveChangesAsync();
    }

    public async void AddRange(IEnumerable<Contact> contacts)
    {
        _dbContext.AddRange(contacts);
        await _dbContext.SaveChangesAsync();
    }

    public async void Delete(Contact contact)
    {
        _dbContext.Remove(contact);
        await _dbContext.SaveChangesAsync();
    }

    public async void DeleteRange(IEnumerable<Contact> contacts)
    {
        _dbContext.RemoveRange(contacts);
        await _dbContext.SaveChangesAsync();
    }

    public void Update(Contact entity)
    {
        throw new System.NotImplementedException();
    }
}