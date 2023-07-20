namespace MyJournalLibrary.Repositories.EntityRepositories;

public interface IEntityRepository<T>
{
    T? GetById(int id);
    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    void Update(T oldEntity, T newEntity);
    void UpdateRange(IEnumerable<T> entities);
}