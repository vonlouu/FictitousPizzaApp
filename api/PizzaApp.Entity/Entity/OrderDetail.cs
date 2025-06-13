using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Entity.Entity
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string PizzaId { get; set; } = null!;
        public int Quantity { get; set; }

        public Order Order { get; set; } = null!;
        public Pizza Pizza { get; set; } = null!;
    }
}
