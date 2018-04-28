using System.Collections.Generic;

namespace UI.Data.Repositories
{
    public interface IGenericRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetById(int entityId);
        void Add(T entity);
        void Remove(T entity);
        void Save();
        bool HasChanges();
        
    }
}