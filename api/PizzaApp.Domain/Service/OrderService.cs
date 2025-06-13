using Microsoft.EntityFrameworkCore;
using PizzaApp.Domain.Dto;
using PizzaApp.Domain.DTO;
using PizzaApp.Domain.Interface;
using PizzaApp.Entity.Entity;
using PizzaApp.Entity.Repository;

public class OrderService : IOrderService
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = _unitOfWork.GetRepository<Order>();
    }

    public async Task<PaginatedResult<OrderSummaryDto>> GetAllOrdersAsync(
        int page = 1,
        int pageSize = 50,
        string sortBy = "OrderDate",
        bool ascending = false,
        string? searchQuery = null)
    {
        IQueryable<Order> query = _orderRepository.Query()
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Pizza)
            .ThenInclude(p => p.PizzaType);


        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            if (int.TryParse(searchQuery, out int orderId))
            {
                query = query.Where(o => o.Id == orderId);
            }
            else
            {
                query = query.Where(o =>
                    o.OrderDetails.Any(od =>
                        od.Pizza.PizzaType.Name.Contains(searchQuery)));
            }
        }

            query = sortBy.ToLower() switch
        {
            "orderdate" => ascending ? query.OrderBy(o => o.Date) : query.OrderByDescending(o => o.Date),
            "totalitems" => ascending
                ? query.OrderBy(o => o.OrderDetails.Sum(od => od.Quantity))
                : query.OrderByDescending(o => o.OrderDetails.Sum(od => od.Quantity)),
            "totalamount" => ascending
            ? query.OrderBy(o => o.OrderDetails.Sum(od => od.Quantity * od.Pizza.Price))
            : query.OrderByDescending(o => o.OrderDetails.Sum(od => od.Quantity * od.Pizza.Price)),
            _ => query.OrderByDescending(o => o.Date)
        };

        var totalItems = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PaginatedResult<OrderSummaryDto>
        {
            Items = items.Select(o => new OrderSummaryDto
            {
                OrderId = o.Id,
                OrderDate = o.Date,
                OrderTime = o.Time,
                TotalItems = o.OrderDetails.Sum(od => od.Quantity),
                TotalAmount = o.OrderDetails.Sum(od => od.Quantity * od.Pizza.Price),
                Pizzas = o.OrderDetails.Select(od => new PizzaItemDto
                {
                    Name = od.Pizza.PizzaType.Name,
                    Size = od.Pizza.Size,
                    Category = od.Pizza.PizzaType.Category,
                    Price = od.Pizza.Price,
                    Quantity = od.Quantity
                }).ToList()
            }),
            TotalItems = totalItems,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<OrderDetailDto?> GetOrderDetailsAsync(int orderId)
    {
        var order = await _orderRepository.Query()
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Pizza)
                    .ThenInclude(p => p.PizzaType)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null) return null;

        var items = order.OrderDetails.Select(od => new PizzaItemDto
        {
            Name = od.Pizza.PizzaType.Name,
            Size = od.Pizza.Size,
            Category = od.Pizza.PizzaType.Category,
            Price = od.Pizza.Price,
            Quantity = od.Quantity
        }).ToList();

        return new OrderDetailDto
        {
            OrderId = order.Id,
            OrderDate = order.Date,
            OrderTime = order.Time,
            Items = items,
            TotalItems = items.Sum(i => i.Quantity),
            TotalAmount = items.Sum(i => i.Quantity * i.Price)
        };
    }

    public async Task<IEnumerable<OrderSummaryDto>> SearchOrdersAsync(string? date, string? query)
    {
        IQueryable<Order> ordersQuery = _orderRepository.Query()
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Pizza)
                    .ThenInclude(p => p.PizzaType);

        if (!string.IsNullOrWhiteSpace(date) && DateTime.TryParse(date, out var parsedDate))
        {
            ordersQuery = ordersQuery.Where(o => EF.Functions.DateDiffDay(o.Date, parsedDate) == 0);
        }

        if (!string.IsNullOrWhiteSpace(query))
        {
            ordersQuery = ordersQuery.Where(o =>
                o.OrderDetails.Any(od => od.Pizza.PizzaType.Name.Contains(query)));
        }

        var orders = await ordersQuery.ToListAsync();

        return orders.Select(o => new OrderSummaryDto
        {
            OrderId = o.Id,
            OrderDate = o.Date,
            OrderTime = o.Time,
            TotalItems = o.OrderDetails.Sum(od => od.Quantity),
            TotalAmount = o.OrderDetails.Sum(od => od.Quantity * od.Pizza.Price)
        });
    }
}
