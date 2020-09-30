using System;
using System.Collections.Generic;

namespace Matrip.Web.Repositories.Contracts
{
    public interface IBaseRepository<TEntity> : IDisposable where TEntity : class
    {
        void Add(TEntity entity);
        void AddWIthChanges(TEntity entity);
        void AddList(List<TEntity> entity);
        TEntity AddAndReturn(TEntity entity);
        TEntity GetById(int id);
        IEnumerable<TEntity> GetAll();
        void Update(TEntity entity);
        void Remove(TEntity entity);
        void RemoveList(List<TEntity> entityList);
        void Save();
    }
}
