namespace FastFood.DataProcessor
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using FastFood.Data;
    using FastFood.DataProcessor.Dto.Export;
    using FastFood.Models.Enums;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;
    public class Serializer
    {
        public static string ExportOrdersByEmployee(FastFoodDbContext context, string employeeName, string orderType)
        {
            var type = (OrderType)Enum.Parse(typeof(OrderType), orderType);

            var orders = context
                .Employees
                .Include(e => e.Orders)
                    .ThenInclude(o => o.OrderItems)
                    .ThenInclude(oi => oi.Item)
                .Where(e => e.Name == employeeName && e.Orders.Any(o => o.Type == type))
                .ToArray()
                .Select(e => new
                {
                    Name = e.Name,
                    Orders = e.Orders.ToArray()
                })
                .Select(e => new
                {
                    Name = e.Name,
                    Orders = e.Orders.Select(o => new
                    {
                        o.Customer,
                        Items = o.OrderItems.Select(oi => new
                        {
                            Name = oi.Item.Name,
                            Price = oi.Item.Price,
                            Quantity = oi.Quantity
                        }).ToArray(),
                        TotalPrice = o.OrderItems.Sum(oi => oi.Item.Price * oi.Quantity)
                    })
                    .OrderByDescending(o => o.TotalPrice)
                    .ToArray()
                })
                .Select(e => new
                {
                    Name = e.Name,
                    Orders = e.Orders
                    .OrderByDescending(o => o.TotalPrice)
                    .ThenByDescending(i => i.Items.Count()),
                    TotalMade = e.Orders.Sum(o => o.TotalPrice)
                })
                .SingleOrDefault();


            var serialized = JsonConvert.SerializeObject(orders, Formatting.Indented);

            return serialized;
        }

        public static string ExportCategoryStatistics(FastFoodDbContext context, string categoriesString)
        {

            var categoriesAsArray = categoriesString.Split(",").ToArray();

            var cagetories = context
                .Categories
                .Include(c => c.Items)
                .Where(c => categoriesAsArray.Contains(c.Name))
                .Select(c => new
                {
                    Name = c.Name,
                    Items = c.Items.ToArray()
                })
                .ToArray()
                .Select(c => new
                {
                    Name = c.Name,
                    MostPopularItem = c.Items
                    .Select(i => new
                    {
                        Name = i.Name,
                        TotalMade = i.Price * i.OrderItems.Sum(oi => oi.Quantity),
                        TimesSold = i.OrderItems.Sum(oi => oi.Quantity)
                    })
                    .ToArray()
                    .OrderByDescending(i => i.TotalMade)
                    .ThenByDescending(i => i.TimesSold)
                    .FirstOrDefault()
                })
                .Select(c => new CategoryDtoXml
                {
                    Name = c.Name,
                    MostPopularItem = new MostPopularItemDtoXml
                    {
                        Name = c.MostPopularItem.Name,
                        TotalMade = c.MostPopularItem.TotalMade,
                        TimesSold = c.MostPopularItem.TimesSold
                    }
                })
                .OrderByDescending(c => c.MostPopularItem.TotalMade)
                .ThenByDescending(c => c.MostPopularItem.TimesSold)
                .ToArray();

            var sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(CategoryDtoXml[]), new XmlRootAttribute("Categories"));
            serializer.Serialize(new StringWriter(sb), cagetories, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));

            var result = sb.ToString();
            return result;
        }
    }
}