using PizzaApp.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Entity.Data
{
    public class PizzaMapper : IPizzaMapper
    {
        public PizzaType MapToEntity(PizzaTypeImportDto dto)
        {
            return new PizzaType
            {
                Id = dto.PizzaTypeId,
                Name = dto.Name,
                Category = dto.Category,
                Ingredients = dto.Ingredients
            };
        }

        public Pizza MapToEntity(PizzaImportDto dto)
        {
            return new Pizza
            {
                Id = dto.PizzaId,
                PizzaTypeId = dto.PizzaTypeId,
                Size = dto.Size,
                Price = decimal.Parse(dto.Price)
            };
        }

        public Order MapToEntity(OrderImportDto dto)
        {
       
            if (!DateTime.TryParse(dto.Date, out var orderDate))
            {
                throw new FormatException($"Invalid Date format: {dto.Date}");
            }

            if (!TimeSpan.TryParse(dto.Time, out var orderTime))
            {
                throw new FormatException($"Invalid Time format: {dto.Time}");
            }

            return new Order
            {
                Date = orderDate,
                Time = orderTime
            };
        }

        public OrderDetail MapToEntity(OrderDetailsImportDto dto)
        {
            if (!int.TryParse(dto.OrderId, out var orderId))
            {
                throw new FormatException($"Invalid OrderId format: {dto.OrderId}");
            }
            if (!int.TryParse(dto.Quantity, out var quanity))
            {
                throw new FormatException($"Invalid OrderId format: {dto.OrderDetailsId}");
            }


            return new OrderDetail
            {
                OrderId = orderId,
                PizzaId = dto.PizzaId,
                Quantity = quanity
            };
        }

        public TEntity MapToEntity<TEntity>(object dto) where TEntity : class
        {
            object entity = dto switch
            {
                PizzaTypeImportDto typeDto => MapToEntity(typeDto),
                PizzaImportDto pizzaDto => MapToEntity(pizzaDto),
                OrderImportDto orderDto => MapToEntity(orderDto),
                OrderDetailsImportDto orderDetailsDto => MapToEntity(orderDetailsDto),
                _ => throw new InvalidOperationException($"No mapping implemented for DTO type {dto.GetType().Name}")
            };

            if (entity is not TEntity typedEntity)
            {
                throw new InvalidCastException($"Mapped entity is not of type {typeof(TEntity).Name}. Actual type: {entity.GetType().Name}");
            }

            return typedEntity;
        }
    }
}
