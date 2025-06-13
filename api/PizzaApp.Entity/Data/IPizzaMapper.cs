using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Entity.Data
{
    public interface IPizzaMapper
    {
        TEntity MapToEntity<TEntity>(object dto) where TEntity : class;
    }
}
