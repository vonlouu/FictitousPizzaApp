using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Entity.Entity
{
    public class PizzaType
    {
        [Key]
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;

        public string Ingredients { get; set; } = null!;

        public ICollection<Pizza> Pizzas { get; set; } = new List<Pizza>();
    }
}
