using System.Collections.Generic;

namespace MyJournal.Models.Repositories.Interfaces;

public interface IEntityRepository<T>
{ 
    T GetById(int id);
    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);
    void Update(T entity);
}