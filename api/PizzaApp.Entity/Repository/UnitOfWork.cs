using Microsoft.Extensions.DependencyInjection;
using PizzaApp.Entity.Context;
using PizzaApp.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Entity.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IServiceProvider _serviceProvider;
        public PizzaDbContext Context { get; }
        private bool _disposed;
        private Dictionary<Type, object> _repositories = new();

        public UnitOfWork(PizzaDbContext context, IServiceProvider serviceProvider)
        {
            Context = context;
            _disposed = false;
            _serviceProvider = serviceProvider;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                var repo = _serviceProvider.GetRequiredService<IRepository<TEntity>>();
                _repositories[type] = repo;
            }
            return (IRepository<TEntity>)_repositories[type];
        }

        public async Task<int> CompleteAsync()
        {
            try
            {
                return await Context.SaveChangesAsync();
            }
            catch (DBConcurrencyException)
            {
                return -1;
            }
            catch (Exception)
            {
                return -2;
            }

        }


        public void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    Context.Dispose();
                }
            }
            _disposed = true;
        }
    }
}
