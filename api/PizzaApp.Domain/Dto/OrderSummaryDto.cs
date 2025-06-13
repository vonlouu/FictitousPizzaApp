using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Domain.Dto
{
    public class OrderSummaryDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public TimeSpan OrderTime { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalItems { get; set; }
        public List<PizzaItemDto> Pizzas { get; set; } = [];
    }
}
