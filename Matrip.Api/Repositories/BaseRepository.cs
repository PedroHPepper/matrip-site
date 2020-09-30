using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Matrip.Web.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        /*Classe que serve de base para toda a manipulação de dados das entidades!!!
         * 
         * Ela permite ser invocada e usar as entidades de forma genérica e contém todos os tipos do CRUD
         * 
         * No contrutor de cada classe filha deve ser injetado a classe de acesso ao banco de dados ApplicationDbContext e ser enviado para a base/
         * (que é esta classe aqui)
         */
        protected readonly ApplicationDbContext _DbContext;
        public BaseRepository(ApplicationDbContext ApplicationDbContext)
        {
            _DbContext = ApplicationDbContext;
        }

        public void Add(TEntity entity)
        {
            _DbContext.Set<TEntity>().Add(entity);
            //_DbContext.SaveChanges();
        }
        public void AddWIthChanges(TEntity entity)
        {
            _DbContext.Set<TEntity>().Add(entity);
            _DbContext.SaveChanges();
        }
        public void AddList(List<TEntity> entity)
        {
            _DbContext.Set<TEntity>().AddRange(entity);
            //_DbContext.SaveChanges();
        }

        public TEntity AddAndReturn(TEntity entity)
        {
            _DbContext.Set<TEntity>().Add(entity);
            //_DbContext.SaveChanges();

            return entity;
        }

        public void Update(TEntity entity)
        {
            _DbContext.Set<TEntity>().Update(entity);
            //_DbContext.SaveChanges();
        }

        public TEntity GetById(int id)
        {
            return _DbContext.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            var query = _DbContext.Set<TEntity>();
            if (query.Any())
            {
                return query.ToList();
            }
            return null;
        }

        public void RemoveById(int id)
        {
            TEntity entity = GetById(id);
            Remove(entity);
        }
        public void Remove(TEntity entity)
        {
            _DbContext.Remove(entity);
            //_DbContext.SaveChanges();
        }

        public void RemoveList(List<TEntity> entityList)
        {
            _DbContext.RemoveRange(entityList);
            //_DbContext.SaveChanges();
        }

        public void Save()
        {
            _DbContext.SaveChanges();
        }

        public void Dispose()
        {
            _DbContext.Dispose();
        }

    }
}
