using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Entity.Entity
{
    public class Pizza
    {
        [Key]
        public string Id { get; set; } = null!;
        public string PizzaTypeId { get; set; } = null!;
        public string Size { get; set; } = null!;
        [Precision(18, 2)]
        public decimal Price { get; set; }
        public PizzaType PizzaType { get; set; } = null!;
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
