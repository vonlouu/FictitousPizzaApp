using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PizzaApp.Entity.Context;
using PizzaApp.Entity.Data;

var builder = WebApplication.CreateBuilder(args);


var configuration = builder.Configuration;

builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddDbContext<PizzaDbContext>(opt =>
{
    opt.UseSqlServer(configuration.GetConnectionString("pizzaApp.DB"));
});
builder.Services.AddScoped<IPizzaMapper, PizzaMapper>();

var app = builder.Build();

// perform seeder
using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<PizzaDbContext>();
        var mapper = scope.ServiceProvider.GetRequiredService<IPizzaMapper>();
        var seeder = new PizzaDataSeeder(dbContext, mapper);

        await seeder.PerformSeedInsertion();
    }
    catch (Exception ex)
    {
       
        Console.WriteLine($"[Seeder Error] {ex.Message}");
        Console.WriteLine(ex.StackTrace);
    }
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();