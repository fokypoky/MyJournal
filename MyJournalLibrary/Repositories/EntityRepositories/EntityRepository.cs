using Microsoft.EntityFrameworkCore;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class EntityRepository<T> : IEntityRepository<T> where T : class
{
    protected readonly DbContext _context;
    
    public EntityRepository(DbContext context)
    {
        _context = context;
    }

    public virtual T GetById(int id) => _context.Set<T>().Find(id);

    public virtual async void Add(T entity)
    {
        _context.Add(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async void AddRange(IEnumerable<T> entities)
    {
        _context.AddRange(entities);
        await _context.SaveChangesAsync();
    }

    public virtual async void Remove(T entity)
    {
        _context.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async void RemoveRange(IEnumerable<T> entities)
    {
        _context.RemoveRange(entities);
        await _context.SaveChangesAsync();
    }
    public virtual async void Update(T oldEntity, T newEntity) {}
}