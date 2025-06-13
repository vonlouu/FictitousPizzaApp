using Microsoft.AspNetCore.Mvc;
using PizzaApp.Domain.Interface;

namespace PizzaApp.Api.Controllers
{
    [ApiController]
    [Route("api/insights")]
    public class InsightsController : Controller
    {
        private readonly IInsightService _insightService;

        public InsightsController(IInsightService insightService)
        {
            _insightService = insightService;
        }

        [HttpGet("top-pizzas")]
        public async Task<IActionResult> GetTopPizzas() =>
        Ok(await _insightService.GetTopPizzasAsync());

        [HttpGet("sales-by-category")]
        public async Task<IActionResult> GetSalesByCategory() =>
            Ok(await _insightService.GetSalesByCategoryAsync());

        [HttpGet("sales-summary")]
        public async Task<IActionResult> GetSalesSummary([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate) =>
            Ok(await _insightService.GetSalesSummaryAsync(startDate, endDate));
    }
}
