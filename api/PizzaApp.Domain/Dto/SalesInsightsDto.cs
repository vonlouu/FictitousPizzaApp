using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Domain.Dto
{
    public class SalesInsightsDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public List<MonthlySalesDto> MonthlySalesTrend { get; set; } = [];
    }

    public class MonthlySalesDto
    {
        public DateTime Date { get; set; }
        public decimal TotalSales { get; set; }
    }

    public class TopPizzaDto
    {
        public string Name { get; set; } = string.Empty;
        public int TotalSold { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
