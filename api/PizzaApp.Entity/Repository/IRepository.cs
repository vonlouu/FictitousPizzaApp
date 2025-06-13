using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Entity.Repository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Query();
    }
}
