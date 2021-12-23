using System;
using System.Collections;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _context;
        private Hashtable _repositories;
        public UnitOfWork(StoreContext context)
        {
            _context = context;
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if(_repositories == null) _repositories = new Hashtable();

            var typeName = typeof(TEntity).Name;

            if(!_repositories.ContainsKey(typeName))
            {
                var repoType = typeof(GenericRepository<>);
                var repoInstance = Activator.CreateInstance(repoType.MakeGenericType(typeof(TEntity)), _context);
                _repositories.Add(typeName, repoInstance);
            }

            return (IGenericRepository<TEntity>)_repositories[typeName];
        }
    }
}