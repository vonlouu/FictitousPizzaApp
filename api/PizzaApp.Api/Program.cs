using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PizzaApp.Domain.Interface;
using PizzaApp.Domain.Service;
using PizzaApp.Entity.Context;
using PizzaApp.Entity.Data;
using PizzaApp.Entity.Repository;

var builder = WebApplication.CreateBuilder(args);


var configuration = builder.Configuration;


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); 


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Pizza API", Version = "v1" });
});


builder.Services.AddDbContext<PizzaDbContext>(opt =>
{
    opt.UseSqlServer(configuration.GetConnectionString("pizzaApp.DB"));
});


builder.Services.AddScoped<IPizzaMapper, PizzaMapper>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IInsightService, InsightService>();

builder.Services.AddCors(options =>
{
options.AddPolicy("CorsPolicy-public",
    builder => builder
        .WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader());

});
var app = builder.Build();

// Database seeding from csv file
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
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database");
    }
}


app.UseCors("CorsPolicy-public");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pizza API v1");
        c.RoutePrefix = "swagger"; 
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();