using Microsoft.EntityFrameworkCore;
using PizzaApp.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Entity.Context
{
    public class PizzaDbContext(DbContextOptions<PizzaDbContext> options) : DbContext(options)
    {
        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<PizzaType> PizzaTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pizza>()
            .HasKey(p => p.Id);

            modelBuilder.Entity<PizzaType>()
             .HasKey(pt => pt.Id);

            modelBuilder.Entity<Pizza>()
            .HasOne(p => p.PizzaType)
            .WithMany(pt => pt.Pizzas)
            .HasForeignKey(p => p.PizzaTypeId);

            modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Order)
            .WithMany(o => o.OrderDetails)
            .HasForeignKey(od => od.OrderId);

            modelBuilder.Entity<OrderDetail>()
             .HasOne(od => od.Pizza)
             .WithMany(p => p.OrderDetails)
             .HasForeignKey(od => od.PizzaId);
        }
    }
}
