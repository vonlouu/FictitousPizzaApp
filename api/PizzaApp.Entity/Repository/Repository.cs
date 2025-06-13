using Microsoft.EntityFrameworkCore;
using PizzaApp.Entity.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Entity.Repository
{
    public class Repository<T>: IRepository<T> where T : class
    {
        protected readonly PizzaDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(PizzaDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IQueryable<T> Query() => _dbSet.AsQueryable();

    }
}
