using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PizzaApp.Domain.Interface;
using PizzaApp.Entity.Context;
using PizzaApp.Entity.Data;
using PizzaApp.Entity.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
var configuration = builder.Configuration;

// Add controllers and API explorer for Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Required for Swagger

// Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Pizza API", Version = "v1" });
});

// Database configuration
builder.Services.AddDbContext<PizzaDbContext>(opt =>
{
    opt.UseSqlServer(configuration.GetConnectionString("pizzaApp.DB"));
});

// Dependency Injection
builder.Services.AddScoped<IPizzaMapper, PizzaMapper>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddCors(options =>
{
options.AddPolicy("CorsPolicy-public",
    builder => builder
        .WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader());

});
var app = builder.Build();

// Database seeding
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

// Configure the HTTP request pipeline
app.UseCors("CorsPolicy-public");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pizza API v1");
        c.RoutePrefix = "swagger"; // Changed from string.Empty to "swagger"
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();