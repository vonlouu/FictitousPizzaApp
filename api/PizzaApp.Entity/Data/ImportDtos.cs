using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Entity.Data
{
    public class PizzaTypeImportDto
    {
        [Name("pizza_type_id")]
        public string PizzaTypeId { get; set; } = null!;

        [Name("name")]
        public string Name { get; set; } = null!;

        [Name("category")]
        public string Category { get; set; } = null!;

        [Name("ingredients")]
        public string Ingredients { get; set; } = null!;
    }

    public class PizzaImportDto
    {
        [Name("pizza_id")]
        public string PizzaId { get; set; } = null!;

        [Name("pizza_type_id")]
        public string PizzaTypeId { get; set; } = null!;

        [Name("size")]
        public string Size { get; set; } = null!;

        [Name("price")]
        public string Price { get; set; } = null!;
    }

    public class OrderImportDto
    {
        [Name("date")]
        public string Date { get; set; } = null!;

        [Name("time")]
        public string Time { get; set; } = null!;
     }

    public class OrderDetailsImportDto
    {
        [Name("order_details_id")]
        public string OrderDetailsId { get; set; } = null!;

        [Name("order_id")]
        public string OrderId { get; set; } = null!;
        [Name("pizza_id")]
        public string PizzaId { get; set; } = null!;

        [Name("quantity")]
        public string Quantity { get; set; } = null!;
    }
}
