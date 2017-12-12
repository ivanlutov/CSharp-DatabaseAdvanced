namespace FastFood.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using FastFood.Data;
    using FastFood.DataProcessor.Dto.Import;
    using FastFood.Models;
    using FastFood.Models.Enums;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    public static class Deserializer
    {
        private const string FailureMessage = "Invalid data format.";
        private const string SuccessMessage = "Record {0} successfully imported.";

        public static string ImportEmployees(FastFoodDbContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var validEmployees = new List<Employee>();
            var validPositions = new List<Position>();

            var deserializedEmployees = JsonConvert.DeserializeObject<ImportEmployeeDto[]>(jsonString);

            foreach (var empDto in deserializedEmployees)
            {
                if (!IsValid(empDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Position position = null;

                var isAlreadyExistPosition = validPositions.Any(p => p.Name == empDto.Position);
                if (isAlreadyExistPosition)
                {
                    position = validPositions.FirstOrDefault(p => p.Name == empDto.Position);
                }
                else
                {
                    position = new Position
                    {
                        Name = empDto.Position
                    };

                    validPositions.Add(position);
                }

                var employee = new Employee
                {
                    Name = empDto.Name,
                    Position = position,
                    Age = empDto.Age
                };

                validEmployees.Add(employee);
                sb.AppendLine(string.Format(SuccessMessage, $"{employee.Name}"));
            }

            context.Employees.AddRange(validEmployees);
            context.Positions.AddRange(validPositions);
            context.SaveChanges();

            var result = sb.ToString();
            return result;

        }

        public static string ImportItems(FastFoodDbContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var validItems = new List<Item>();
            var validCategories = new List<Category>();

            var deserializedItems = JsonConvert.DeserializeObject<ImportItemDto[]>(jsonString);

            foreach (var itemDto in deserializedItems)
            {
                if (!IsValid(itemDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var isItemAlreadyExist = validItems.Any(i => i.Name == itemDto.Name);
                if (isItemAlreadyExist)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Category category = null;
                var isCategoryAlreadyExist = validCategories.Any(c => c.Name == itemDto.Category);

                if (isCategoryAlreadyExist)
                {
                    category = validCategories.FirstOrDefault(c => c.Name == itemDto.Category);
                }
                else
                {
                    category = new Category
                    {
                        Name = itemDto.Category
                    };

                    validCategories.Add(category);
                }

                var item = new Item
                {
                    Name = itemDto.Name,
                    Category = category,
                    Price = itemDto.Price
                };

                validItems.Add(item);
                sb.AppendLine(string.Format(SuccessMessage, $"{item.Name}"));

            }

            context.Categories.AddRange(validCategories);
            context.Items.AddRange(validItems);
            context.SaveChanges();

            var result = sb.ToString();
            return result;
        }

        public static string ImportOrders(FastFoodDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ImportOrderXmlDto[]), new XmlRootAttribute("Orders"));
            var deserializedOrders = (ImportOrderXmlDto[])serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));
            var validOrders = new List<Order>();

            var sb = new StringBuilder();

            foreach (var orderDto in deserializedOrders)
            {
                if (!IsValid(orderDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var isOrderEmployeeExist = context.Employees.Any(e => e.Name == orderDto.Employee);
                if (!isOrderEmployeeExist)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var items = orderDto.Items.ToArray();
                var isValidItems = items.All(i => IsValid(items));
                if (!isValidItems)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var isItemsExist = orderDto.Items
                    .All(s => context.Items.Any(i => i.Name == s.Name));

                if (!isItemsExist)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var date = DateTime.ParseExact(orderDto.DateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                OrderType type = OrderType.ForHere;
                if (orderDto.Type != null)
                {
                    type = (OrderType)Enum.Parse(typeof(OrderType), orderDto.Type, true);
                }

                var orderItems = new List<OrderItem>();
                foreach (var itemDto in orderDto.Items)
                {
                    var item = context.Items.First(i => i.Name == itemDto.Name);
                    var orderItem = new OrderItem()
                    {
                        Item = item,
                        Quantity = itemDto.Quantity
                    };

                    orderItems.Add(orderItem);
                }

                var totalPrice = orderItems.Sum(oi => oi.Item.Price);

                var employee = context.Employees.FirstOrDefault(e => e.Name == orderDto.Employee);
                var order = new Order
                {
                    Employee = employee,
                    Type = type,
                    DateTime = date,
                    TotalPrice = totalPrice,
                    Customer = orderDto.Customer,
                    OrderItems = orderItems
                };

                validOrders.Add(order);
                sb.AppendLine($"Order for {orderDto.Customer} on {date.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)} added");
            }

            context.Orders.AddRange(validOrders);
            context.SaveChanges();

            var result = sb.ToString();
            return result;
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
            return isValid;
        }
    }
}