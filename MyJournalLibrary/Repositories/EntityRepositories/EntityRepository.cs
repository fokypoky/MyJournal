using Microsoft.EntityFrameworkCore;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class EntityRepository<T> : IEntityRepository<T> where T : class
{
    protected readonly DbContext _context;
    
    public EntityRepository(DbContext context)
    {
        _context = context;
    }

    public virtual T? GetById(int id) => _context.Set<T>().Find(id);

    public virtual void Add(T entity)
    {
        _context.Add(entity);
        _context.SaveChanges();
    }

    public virtual void AddRange(IEnumerable<T> entities)
    {
        _context.AddRange(entities);
        _context.SaveChanges();
    }

    public virtual void Remove(T entity)
    {
        _context.Remove(entity);
        _context.SaveChanges();
    }

    public virtual void RemoveRange(IEnumerable<T> entities)
    {
        _context.RemoveRange(entities);
        _context.SaveChanges();
    }

    public virtual void Update(T entity)
    {
        _context.Update(entity);
        _context.SaveChanges();
    }

    public virtual void UpdateRange(IEnumerable<T> entities)
    {
        _context.UpdateRange(entities);
        _context.SaveChanges();
    }
}