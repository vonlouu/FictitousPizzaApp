using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Domain.Dto
{
    public class PizzaItemDto
    {
        public string Name { get; set; } = null!;
        public string Size { get; set; } = null!;
        public string Category { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }

    }
}
