namespace MyJournalLibrary.Repositories.EntityRepositories;

public interface IEntityRepository<T>
{
    T? GetById(int id);
    void Add(T entity);
    void AddRange(List<T> entities);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
}