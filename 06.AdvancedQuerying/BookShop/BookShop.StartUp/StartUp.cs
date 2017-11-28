using Microsoft.EntityFrameworkCore;

namespace BookShop
{
    using BookShop.Data;
    using BookShop.Initializer;
    using BookShop.Models;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Globalization;
    using System.Text.RegularExpressions;

    public class StartUp
    {
        static void Main()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            //using (var db = new BookShopContext())
            //{
            //    DbInitializer.ResetDatabase(db);
            //}

            var context = new BookShopContext();

            using (context)
            {
                var result = GetMostRecentBooks(context);
                Console.WriteLine(result);
            }
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var sb = new StringBuilder();

            var restrictionValue = 0;
            switch (command.ToLower())
            {
                case "minor":
                    restrictionValue = 0;
                    break;
                case "teen":
                    restrictionValue = 1;
                    break;
                case "adult":
                    restrictionValue = 2;
                    break;
                default:
                    throw new ArgumentException();
            }

            context
                .Books
                .Where(b => (int)b.AgeRestriction == restrictionValue)
                .Select(b => b.Title)
                .OrderBy(t => t)
                .ToList()
                .ForEach(t => sb.AppendLine(t));

            return sb.ToString().Trim();
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var sb = new StringBuilder();

            context
                .Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList()
                .ForEach(t => sb.AppendLine(t));

            return sb.ToString().Trim();
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var sb = new StringBuilder();

            context
                .Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => new
                {
                    Title = b.Title,
                    Price = b.Price
                })
                .ToList()
                .ForEach(b => sb.AppendLine($"{b.Title} - ${b.Price:F2}"));

            return sb.ToString().Trim();
        }

        public static string GetBooksNotRealeasedIn(BookShopContext context, int year)
        {
            var sb = new StringBuilder();

            context
                .Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList()
                .ForEach(t => sb.AppendLine(t));

            return sb.ToString().Trim();
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input.ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder();

            context
                .Books
                .Include(b => b.BookCategories)
                .ThenInclude(c => c.Category)
                .Where(b => b.BookCategories.Any(c => categories.Contains(c.Category.Name.ToLower())))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToList()
                .ForEach(t => sb.AppendLine(t));

            return sb.ToString().Trim();
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var dateForCompare = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var sb = new StringBuilder();

            context
                .Books
                .Where(b => b.ReleaseDate < dateForCompare)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    Title = b.Title,
                    Price = b.Price,
                    EditionType = b.EditionType
                })
                .ToList()
                .ForEach(b => sb.AppendLine($"{b.Title} - {b.EditionType} - ${b.Price:F2}"));

            return sb.ToString().Trim();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var sb = new StringBuilder();

            context
                .Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => new
                {
                    Name = string.Concat(a.FirstName, " ", a.LastName)
                })
                .OrderBy(n => n.Name)
                .ToList()
                .ForEach(n => sb.AppendLine(n.Name));

            return sb.ToString().Trim();
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var sb = new StringBuilder();

            context
                .Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToList()
                .ForEach(t => sb.AppendLine(t));

            return sb.ToString().Trim();
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var sb = new StringBuilder();

            context
                .Books
                .Include(b => b.Author)
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    Title = b.Title,
                    AuthorName = string.Concat(b.Author.FirstName, " ", b.Author.LastName)
                })
                .ToList()
                .ForEach(b => sb.AppendLine($"{b.Title} ({b.AuthorName})"));

            return sb.ToString().Trim();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var booksCount = context
                .Books
                .Count(b => b.Title.Length > lengthCheck);

            return booksCount;
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var sb = new StringBuilder();

            context
                .Authors
                .Include(a => a.Books)
                .Select(a => new
                {
                    AuthorName = string.Concat(a.FirstName, " ", a.LastName),
                    NumberOfCopies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(a => a.NumberOfCopies)
                .ToList()
                .ForEach(a => sb.AppendLine($"{a.AuthorName} - {a.NumberOfCopies}"));

            return sb.ToString().Trim();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var sb = new StringBuilder();

            context
                .Categories
                .Include(c => c.CategoryBooks)
                .ThenInclude(c => c.Book)
                .Select(c => new
                {
                    CategoryName = c.Name,
                    Profit = c.CategoryBooks.Sum(p => (p.Book.Copies * p.Book.Price))
                })
                .OrderByDescending(c => c.Profit)
                .ThenBy(c => c.CategoryName)
                .ToList()
                .ForEach(c => sb.AppendLine($"{c.CategoryName} ${c.Profit:F2}"));

            return sb.ToString().Trim();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    categoryBookCount = c.CategoryBooks.Count(),
                    CategoryBooks = c.CategoryBooks
                        .OrderByDescending(x => x.Book.ReleaseDate.Value)
                        .Take(3)
                        .Select(y => new
                            {
                                y.Book.Title,
                                y.Book.ReleaseDate.Value.Year
                            }
                        )
                        .OrderByDescending(x => x.Year)
                })
                .OrderBy(x => x.CategoryName)
                .ToArray();

            return string.Join(Environment.NewLine, categories.Select(x => $"--{x.CategoryName}" + Environment.NewLine + string.Join(Environment.NewLine, x.CategoryBooks.Select(y => $"{y.Title} ({y.Year})"))));
        }

        public static void IncreasePrices(BookShopContext context)
        {
            context
                .Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToList()
                .ForEach(b => b.Price += 5);

            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var booksForDelete = context
                .Books
                .Where(b => b.Copies < 4200)
                .ToList();

            var countOfBooks = booksForDelete.Count;

            context.
                Books
                .RemoveRange(booksForDelete);

            context.SaveChanges();

            return countOfBooks;
        }
    }
}
