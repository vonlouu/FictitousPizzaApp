using Microsoft.EntityFrameworkCore;
using PizzaApp.Domain.Dto;
using PizzaApp.Domain.Interface;
using PizzaApp.Entity.Entity;
using PizzaApp.Entity.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Domain.Service
{
    public class InsightService : IInsightService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InsightService(IRepository<Order> orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<object>> GetSalesByCategoryAsync()
        {
            var orders = await _orderRepository.Query()
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Pizza)
                    .ThenInclude(p => p.PizzaType)
            .ToListAsync();

            return orders
                .SelectMany(o => o.OrderDetails)
                .GroupBy(od => od.Pizza.PizzaType.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    TotalRevenue = g.Sum(x => x.Quantity * x.Pizza.Price)
                })
                .OrderByDescending(x => x.TotalRevenue)
                .ToList();
        }

        public async Task<SalesInsightsDto> GetSalesSummaryAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            IQueryable<Order> query = _orderRepository.Query()
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Pizza)
                    .ThenInclude(p => p.PizzaType);

            if (startDate.HasValue)
                query = query.Where(o => o.Date >= startDate.Value.Date);

            if (endDate.HasValue)
                query = query.Where(o => o.Date <= endDate.Value.Date);

            var orders = await query.ToListAsync();

            var totalRevenue = orders.Sum(o =>
                o.OrderDetails.Sum(od => od.Quantity * od.Pizza.Price));

            var totalOrders = orders.Count;

            var monthlySales = orders
                .GroupBy(o => new { o.Date.Year, o.Date.Month })
                .Select(g => new MonthlySalesDto
                {
                    Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                    TotalSales = g.Sum(o => o.OrderDetails.Sum(od => od.Quantity * od.Pizza.Price))
                })
                .OrderBy(x => x.Date)
                .ToList();

            return new SalesInsightsDto
            {
                TotalRevenue = totalRevenue,
                TotalOrders = totalOrders,
                MonthlySalesTrend = monthlySales
            };
        }

        public async Task<IEnumerable<TopPizzaDto>> GetTopPizzasAsync()
        {
            var orders = await _orderRepository.Query()
           .Include(o => o.OrderDetails)
               .ThenInclude(od => od.Pizza)
                   .ThenInclude(p => p.PizzaType)
           .ToListAsync();

            return orders
             .SelectMany(o => o.OrderDetails)
             .GroupBy(od => od.Pizza.PizzaType.Name)
             .Select(g => new TopPizzaDto
                {
                  Name = g.Key,
                  TotalSold = g.Sum(x => x.Quantity),
                  TotalRevenue = g.Sum(x => x.Quantity * x.Pizza.Price)
                })
             .OrderByDescending(p => p.TotalSold)
             .Take(5)
             .ToList();
        }
    }
}
