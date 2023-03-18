using Microsoft.EntityFrameworkCore;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class EntityRepository<T> : IEntityRepository<T>
{
    protected readonly DbContext _context;
    
    public EntityRepository(DbContext context)
    {
        _context = context;
    }

    public virtual T GetById(int id) => default(T);
    public virtual async void Add(T entity) {}
    public virtual async void AddRange(IEnumerable<T> entities) {}
    public virtual async void Remove(T entity) {}
    public virtual async void RemoveRange(IEnumerable<T> entities) {}
    public virtual async void Update(T oldEntity, T newEntity) {}
}