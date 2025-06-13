using PizzaApp.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Domain.Interface
{
    public interface IInsightService
    {
        Task<SalesInsightsDto> GetSalesSummaryAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<IEnumerable<TopPizzaDto>> GetTopPizzasAsync();
        Task<IEnumerable<object>> GetSalesByCategoryAsync();
    }
}
