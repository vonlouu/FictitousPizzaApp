using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PizzaApp.Domain.Interface;

namespace PizzaApp.Api.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {

        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string sortBy = "OrderDate",
            [FromQuery] bool ascending = false,
            [FromQuery(Name = "q")] string? searchQuery = null)
        {

            var result = await _orderService.GetAllOrdersAsync(page, pageSize, sortBy, ascending, searchQuery);
            return Ok(result);
        }
    }
}
