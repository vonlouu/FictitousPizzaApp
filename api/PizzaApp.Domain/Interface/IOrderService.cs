using PizzaApp.Domain.Dto;
using PizzaApp.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Domain.Interface
{
    public interface IOrderService
    {
        Task<PaginatedResult<OrderSummaryDto>> GetAllOrdersAsync(
           int page = 1,
           int pageSize = 50,
           string sortBy = "OrderDate",
           bool ascending = false,
           string? searchQuery = null);

        Task<OrderDetailDto?> GetOrderDetailsAsync(int orderId);
    }
}
