using PizzaApp.Entity.Context;
using PizzaApp.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Entity.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        PizzaDbContext Context { get; }

        Task<int> CompleteAsync();
    }
}
