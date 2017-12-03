namespace ProductShop.Client
{
    using System;
    using ProductShop.Data;
    using System.Collections.Generic;
    using System.IO;
    using Newtonsoft.Json;
    using ProductShop.Models;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using ProductShop.Client.ModelsDtos;
    using System.Xml.Linq;


    public class Application
    {
        public static void Main()
        {
            ResetDatabase();
            InitializeMapper();

            CreateDirectories();

            ImportDataJson();
            ExportJson();

            ImportDataXml();
            ExportXml();
        }

        private static void CreateDirectories()
        {
            if (!Directory.Exists("ExportResult"))
            {
                Directory.CreateDirectory("ExportResult");

                if (!Directory.Exists("ExportResult\\Json"))
                {
                    Directory.CreateDirectory("ExportResult\\Json");
                }
                if (!Directory.Exists("ExportResult\\Xml"))
                {
                    Directory.CreateDirectory("ExportResult\\Xml");
                }
            }
        }

        private static void ImportDataJson()
        {
            ImportUsersJson();
            ImportProductsJson();
            ImportCategoriesJson();
        }
        private static void ImportDataXml()
        {
            ImportUsersXml();
            ImportProductsXml();
            ImportCategoriesXml();
        }

        private static void ExportJson()
        {
            //Query 1 - Products In Range
            ExportProductsInRange();

            //Query 2 - Successfully Sold Products
            ExportSuccessfullySoldProducts();

            //Query 3 - Categories By Products Count
            ExportCategoriesByProductsCount();

            //Query 4 - Users and Products
            UsersAndProducts();
        }

        private static void ExportXml()
        {
            //Query 1 - Products In Range
            ExportProductsInRangeXml();

            //Query 2 - Sold Products
            ExportSoldProductsXml();

            //Query 3 - Categories By Products Count
            ExportCategoriesByProductsCountXml();

            //Query 4 - Users and Products
            ExportUsersAndProductsXml();
        }

        private static void ExportUsersAndProductsXml()
        {
            using (ProductShopContext context = new ProductShopContext())
            {
                var users =
                    context
                        .Users
                        .Include(u => u.SoldProducts)
                        .ThenInclude(p => p.Buyer)
                        .Where(u => u.SoldProducts.Count(p => p.Buyer != null) >= 1)
                        .ToArray();

                var xmlArray = new
                {
                    usersCount = users.Length,
                    users = users.Select(u => new
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Age = u.Age,
                        soldProducts = new
                        {
                            Count = u.SoldProducts.Count(p => p.Buyer != null),
                            Products = u.SoldProducts.Where(p => p.Buyer != null).Select(p => new
                            {
                                name = p.Name,
                                price = p.Price
                            })
                                    .ToArray()
                        }
                    }).ToArray()
                        .OrderByDescending(up => up.soldProducts.Count)
                        .ThenBy(up => up.LastName)
                };

                XDocument document = new XDocument();
                document.Add(new XElement("users", new XAttribute("count", $"{xmlArray.usersCount}")));

                foreach (var u in xmlArray.users)
                {
                    document.Root.Add(new XElement("user",
                        new XAttribute("first-name", $"{u.FirstName}"),
                        new XAttribute("last-name", $"{u.LastName}"),
                        new XAttribute("age", $"{u.Age}"),
                            new XElement("sold-products",
                            new XAttribute("count", $"{u.soldProducts.Count}"))));

                    var element =
                        document.Root.Elements()
                        .SingleOrDefault(e => e.Name == "user"
                        && e.Attribute("first-name").Value == $"{u.FirstName}"
                        && e.Attribute("last-name").Value == $"{u.LastName}"
                        && e.Attribute("age").Value == $"{u.Age}")
                        .Elements()
                        .SingleOrDefault(e => e.Name == "sold-products");

                    foreach (var p in u.soldProducts.Products)
                    {
                        element.Add(new XElement("product",
                                        new XAttribute("name", $"{p.name}"),
                                        new XAttribute("price", $"{p.price}")));
                    }
                }

                document.Save("ExportResult\\Xml\\04.UsersAndProductsXml.xml");
            }
        }

        private static void ExportCategoriesByProductsCountXml()
        {
            using (ProductShopContext context = new ProductShopContext())
            {
                var categories = context
                    .CategoryProducts
                        .Include(cp => cp.Category)
                        .Include(cp => cp.Product)
                    .GroupBy(c => c.Category.Name)
                    .Select(g => new CategoryDto
                    {
                        Category = g.Key,
                        Products = g.Select(cp => cp.Product).ToArray()
                    })
                    .ProjectTo<CategoryProductsDto>()
                    .OrderByDescending(c => c.ProductsCount)
                    .ToArray();

                XDocument document = new XDocument();
                document.Add(new XElement("categories"));

                foreach (var c in categories)
                {
                    document.Root.Add(new XElement("category",
                                        new XAttribute("name", $"{c.Category}"),
                                            new XElement("products-count", $"{c.ProductsCount}"),
                                            new XElement("average-price", $"{c.AveragePrice}"),
                                            new XElement("total-revenue", $"{c.TotalRevenue}")));
                }

                document.Save("ExportResult\\Xml\\03.CategoriesWithProductsXml.xml");
            }
        }

        private static void ExportSoldProductsXml()
        {
            using (ProductShopContext context = new ProductShopContext())
            {
                var users = context
                    .Users
                    .Include(u => u.SoldProducts)
                    .ThenInclude(p => p.Buyer)
                    .Where(u => u.SoldProducts.Count(p => p.Buyer != null) >= 1)
                    .Select(u => new
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Products = u.SoldProducts.Where(p => p.Buyer != null)
                            .Select(p => new
                            {
                                Name = p.Name,
                                Price = p.Price
                            })
                            .ToArray()
                    })
                    .OrderBy(u => u.FirstName)
                    .ThenBy(u => u.LastName)
                    .ToArray();

                XDocument document = new XDocument();
                document.Add(new XElement("users"));

                foreach (var user in users)
                {
                    document.Root.Add(
                        new XElement("user",
                            new XAttribute("first-name", $"{user.FirstName}"),
                            new XAttribute("last-name", $"{user.LastName}"),
                                new XElement("sold-products")));

                    var productElements = document.Root.Elements()
                        .SingleOrDefault(e => e.Name == "user"
                                              && e.Attribute("first-name").Value == $"{user.FirstName}"
                                              && e.Attribute("last-name").Value == $"{user.LastName}")
                                                .Element("sold-products");

                    foreach (var p in user.Products)
                    {
                        productElements
                            .Add(new XElement("product",
                                     new XElement("name", $"${p.Name}"),
                                        new XElement("price", $"{p.Price}")));
                    }
                }

                document.Save("ExportResult\\Xml\\02.UsersWithSoldProductsXml.xml");
            }
        }

        private static void ExportProductsInRangeXml()
        {
            using (ProductShopContext context = new ProductShopContext())
            {
                var products = context
                    .Products
                    .Include(p => p.Buyer)
                    .Where(p => p.Price >= 1000 && p.Price <= 2000 && p.Buyer != null)
                    .Select(p => new
                    {
                        productName = p.Name,
                        price = p.Price,
                        buyer = $"{p.Buyer.FirstName} {p.Buyer.LastName}"
                    })
                    .ToArray();

                XDocument document = new XDocument();
                document.Add(
                    new XElement("products"));

                foreach (var product in products)
                {
                    document.Root.Add(
                        new XElement("product",
                        new XAttribute("name", $"{product.productName}"),
                        new XAttribute("price", $"{product.price}"),
                        new XAttribute("buyer", $"{product.buyer}")));
                }

                document.Save("ExportResult\\Xml\\01.ProductsInRangeXml.xml");
            }
        }

        private static void ImportCategoriesXml()
        {
            XDocument document = XDocument.Load("categories.xml");

            var categories = document.Root.Elements();
            var categoriesToDatabase = new List<Category>();
            Random rnd = new Random();

            using (ProductShopContext context = new ProductShopContext())
            {
                foreach (var category in categories)
                {
                    var name = category.Element("name").Value;
                    var currentCategory = new Category()
                    {
                        Name = name
                    };

                    categoriesToDatabase.Add(currentCategory);
                }

                context.Categories.AddRange(categoriesToDatabase);
                context.SaveChanges();

                var categoriesCount = categoriesToDatabase.Count;
                var productsCount = context.Products.Count();

                var categoryProducts = new List<CategoryProduct>();
                for (int i = 1; i < productsCount + 1; i++)
                {
                    var currentCategoryProduct = new CategoryProduct()
                    {
                        ProductId = i,
                        CategoryId = rnd.Next(1, categoriesCount + 1)
                    };

                    categoryProducts.Add(currentCategoryProduct);
                }

                context.CategoryProducts.AddRange(categoryProducts);
                context.SaveChanges();
            }
        }

        private static void ImportProductsXml()
        {
            XDocument document = XDocument.Load("products.xml");
            var products = document.Root.Elements();
            var productsToDatabase = new List<Product>();

            Random rnd = new Random();

            using (ProductShopContext context = new ProductShopContext())
            {
                var usersCount = context.Users.Count();

                foreach (var product in products)
                {
                    var name = product.Element("name").Value;
                    var price = decimal.Parse(product.Element("price").Value);

                    var currentProduct = new Product()
                    {
                        Name = name,
                        Price = price
                    };

                    var sellerId = rnd.Next(1, usersCount + 1);
                    currentProduct.SellerId = sellerId;
                    productsToDatabase.Add(currentProduct);
                }

                context.Products.AddRange(productsToDatabase);
                context.SaveChanges();

                var productsCount = context.Products.Count();
                for (int i = 1; i < productsCount / 4; i++)
                {
                    var currentProduct = context.Products.Find(i);

                    var buyerId = rnd.Next(1, usersCount + 1);
                    while (currentProduct.SellerId == buyerId)
                    {
                        buyerId = rnd.Next(1, usersCount + 1);
                    }

                    currentProduct.BuyerId = buyerId;
                }

                context.SaveChanges();
            }
        }

        private static void ImportUsersXml()
        {
            XDocument document = XDocument.Load("users.xml");

            var users = document.Root.Elements();

            using (ProductShopContext context = new ProductShopContext())
            {
                var usersToDatabase = new List<User>();

                foreach (var user in users)
                {
                    var firstName = user.Attribute("firstName")?.Value;
                    var lastName = user.Attribute("lastName")?.Value;

                    var ageAttribute = user.Attribute("age")?.Value ?? "null";
                    int? age = null;
                    if (ageAttribute != "null")
                    {
                        age = int.Parse(user.Attribute("age").Value);
                    }

                    var currentUser = new User()
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Age = age
                    };

                    usersToDatabase.Add(currentUser);
                }

                context.Users.AddRange(usersToDatabase);
                context.SaveChanges();
            }
        }

        private static void InitializeMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Product, ProductDto>()
                    .ForMember(dto => dto.SellerName,
                        opt => opt.MapFrom(src => $"{src.Seller.FirstName} {src.Seller.LastName}"));
                cfg.CreateMap<Product, SoldProductDto>()
                    .ForMember(dto => dto.BuyerFirstName,
                        opt => opt.MapFrom(src => src.Buyer.FirstName))
                    .ForMember(dto => dto.BuyerLastName,
                        opt => opt.MapFrom(src => src.Buyer.LastName));

                cfg.CreateMap<Product, UserSoldProductsDto>()
                    .ForMember(dto => dto.FirstName,
                        opt => opt.MapFrom(src => src.Seller.FirstName))
                    .ForMember(dto => dto.LastName,
                        opt => opt.MapFrom(src => src.Seller.LastName));

                cfg.CreateMap<CategoryDto, CategoryProductsDto>()
                    .ForMember(dto => dto.ProductsCount,
                        opt => opt.MapFrom(src => src.Products.Count))
                    .ForMember(dto => dto.AveragePrice,
                        opt => opt.MapFrom(src => src.Products.Average(p => p.Price)))
                    .ForMember(dto => dto.TotalRevenue,
                        opt => opt.MapFrom(src => src.Products.Sum(p => p.Price)));
            });
        }

        private static void UsersAndProducts()
        {
            using (ProductShopContext context = new ProductShopContext())
            {
                var users =
                    context
                        .Users
                        .Include(u => u.SoldProducts)
                        .ThenInclude(p => p.Buyer)
                        .Where(u => u.SoldProducts.Count(p => p.Buyer != null) >= 1)
                        .ToArray();

                var jsonArray = new
                {
                    usersCount = users.Length,
                    users = users.Select(u => new
                    {
                        firstName = u.FirstName,
                        lastName = u.LastName,
                        age = u.Age,
                        soldProducts = new
                        {
                            count = u.SoldProducts.Count(p => p.Buyer != null),
                            products = u.SoldProducts.Where(p => p.Buyer != null).Select(p => new
                            {
                                name = p.Name,
                                price = p.Price
                            })
                                    .ToArray()
                        }
                    }).ToArray()
                        .OrderByDescending(up => up.soldProducts.count)
                        .ThenBy(up => up.lastName)
                };

                var userProductsJson = JsonConvert.SerializeObject(jsonArray, Formatting.Indented, new JsonSerializerSettings()
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });

                File.WriteAllText("ExportResult\\Json\\04.UsersWithSoldProductsJson.json", userProductsJson);
            }
        }

        private static void ExportCategoriesByProductsCount()
        {
            using (ProductShopContext context = new ProductShopContext())
            {
                var categories = context
                    .CategoryProducts
                    .Include(cp => cp.Category)
                    .Include(cp => cp.Product)
                    .GroupBy(c => c.Category.Name)
                    .OrderBy(c => c.Key)
                    .Select(g => new CategoryDto()
                    {
                        Category = g.Key,
                        Products = g.Select(cp => cp.Product).ToArray()
                    })
                    .ProjectTo<CategoryProductsDto>()
                    .ToArray();

                var categoriesJson = JsonConvert.SerializeObject(categories, Formatting.Indented, new JsonSerializerSettings()
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });

                File.WriteAllText("ExportResult\\Json\\03.CategoriesByProductsCountJson.json", categoriesJson);
            }
        }

        private static void ExportSuccessfullySoldProducts()
        {
            using (ProductShopContext context = new ProductShopContext())
            {
                var users = context
                    .Users
                    .Include(u => u.SoldProducts)
                    .ThenInclude(p => p.Buyer)
                    .Where(u => u.SoldProducts.Any(p => p.Buyer != null))
                    .Select(u => new
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        SoldProducts = u.SoldProducts.Where(p => p.Buyer != null)
                    })
                    .OrderBy(u => u.LastName)
                    .ThenBy(u => u.FirstName)
                    .ProjectTo<UserSoldProductsDto>()
                    .ToList();

                var usersJson = JsonConvert.SerializeObject(users, Formatting.Indented, new JsonSerializerSettings()
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });

                File.WriteAllText("ExportResult\\Json\\02.UsersWithSoldProductsJson.json", usersJson);
            }
        }

        private static void ExportProductsInRange()
        {
            using (ProductShopContext context = new ProductShopContext())
            {
                var products = context
                    .Products
                    .Where(p => p.Price >= 500 && p.Price <= 1000)
                    .OrderBy(p => p.Price)
                    .ProjectTo<ProductDto>()
                    .ToArray();

                var productsJson = JsonConvert.SerializeObject(products, Formatting.Indented);

                File.WriteAllText("ExportResult\\Json\\01.ProductsInRangeJson.json", productsJson);
            }
        }

        private static void ImportCategoriesJson()
        {
            var categoriesJson = File.ReadAllText("categories.json");
            var categories = JsonConvert.DeserializeObject<Category[]>(categoriesJson);
            Random rnd = new Random();

            using (ProductShopContext context = new ProductShopContext())
            {
                context.Categories.AddRange(categories);
                context.SaveChanges();

                var categoriesCount = categories.Length;
                var productCount = context.Products.Count();
                var categoryProducts = new List<CategoryProduct>();

                for (int i = 1; i < productCount + 1; i++)
                {
                    var categoryId = rnd.Next(1, categoriesCount + 1);
                    var currentCategoryProduct = new CategoryProduct()
                    {
                        ProductId = i,
                        CategoryId = categoryId
                    };

                    categoryProducts.Add(currentCategoryProduct);
                }

                context.CategoryProducts.AddRange(categoryProducts);
                context.SaveChanges();
            }
        }

        private static void ImportProductsJson()
        {
            var productsJson = File.ReadAllText("products.json");
            var products = JsonConvert.DeserializeObject<Product[]>(productsJson);
            Random rnd = new Random();
            using (ProductShopContext context = new ProductShopContext())
            {
                var userCount = context.Users.Count();

                foreach (var product in products)
                {
                    var sellerId = rnd.Next(1, userCount + 1);
                    product.SellerId = sellerId;
                }

                for (int i = 0; i < products.Length / 4; i++)
                {
                    var buyerId = rnd.Next(1, userCount + 1);
                    while (products[i].SellerId == buyerId)
                    {
                        buyerId = rnd.Next(1, userCount + 1);
                    }

                    products[i].BuyerId = buyerId;
                }

                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }

        private static void ImportUsersJson()
        {
            var usersJson = File.ReadAllText("users.json");
            var users = JsonConvert.DeserializeObject<User[]>(usersJson);
            using (ProductShopContext context = new ProductShopContext())
            {
                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }

        private static void ResetDatabase()
        {
            using (ProductShopContext context = new ProductShopContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }
    }
}
