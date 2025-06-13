using PizzaApp.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Domain.DTO
{
    public class OrderDetailDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public TimeSpan OrderTime { get; set; }
        public List<PizzaItemDto> Items { get; set; } = [];
        public decimal TotalAmount { get; set; }
        public int TotalItems { get; set; }
    }
}
