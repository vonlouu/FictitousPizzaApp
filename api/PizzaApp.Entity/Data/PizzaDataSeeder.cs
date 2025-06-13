using CsvHelper;
using Microsoft.EntityFrameworkCore;
using PizzaApp.Entity.Context;
using PizzaApp.Entity.Entity;
using System.Globalization;

namespace PizzaApp.Entity.Data
{
    public class PizzaDataSeeder
    {
        private readonly PizzaDbContext _context;
        private readonly IPizzaMapper _mapper;

        public PizzaDataSeeder(PizzaDbContext context, IPizzaMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task PerformSeedInsertion()
        {
            await SeedData<PizzaType, PizzaTypeImportDto>("pizza_types.csv", _context.PizzaTypes);
            await SeedData<Pizza, PizzaImportDto>("pizzas.csv", _context.Pizzas);
            await SeedData<Order, OrderImportDto>("orders.csv", _context.Orders);
            await SeedData<OrderDetail, OrderDetailsImportDto>("order_details.csv", _context.OrderDetails);
        }

        private async Task SeedData<TEntity, TDto>(string fileName, DbSet<TEntity> dbSet)
            where TEntity : class
            where TDto : class
        {
            try
            {
                if (await dbSet.AnyAsync())
                    return;

                var path = Path.Combine(AppContext.BaseDirectory, "Data", fileName);
                if (!File.Exists(path))
                    throw new FileNotFoundException($"CSV file not found at {path}");

                using var reader = new StreamReader(path);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                var records = csv.GetRecords<TDto>();
                var entities = records.Select(dto => _mapper.MapToEntity<TEntity>(dto));

                await dbSet.AddRangeAsync(entities);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding {typeof(TEntity).Name}: {ex.Message}");
                throw;
            }
        }
    }
}