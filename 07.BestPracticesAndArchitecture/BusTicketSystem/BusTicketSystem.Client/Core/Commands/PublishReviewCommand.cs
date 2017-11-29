namespace BusTicketSystem.Client.Core.Commands
{
    using System;
    using System.Linq;
    using BusTicketSystem.Data;
    using BusTicketSystem.Models;
    using Microsoft.EntityFrameworkCore;
    using BusTicketSystem.Client.Interfaces;

    public class PublishReviewCommand : ICommand
    {
        public string Execute(string[] args)
        {
            var customerId = int.Parse(args[0]);
            var grade = double.Parse(args[1]);
            var busCompanyName = args[2];
            var content = args[3];

            using (BusTicketSystemContext context = new BusTicketSystemContext())
            {
                var customer = context.Customers.Include(c => c.BankAccount).SingleOrDefault(c => c.Id == customerId);
                if (customer == null)
                {
                    throw new ArgumentException($"Customer with {customerId} does not exist!");
                }

                var company = context.BusCompanies.SingleOrDefault(c => c.Name == busCompanyName);
                if (company == null)
                {
                    throw new ArgumentException($"Bus company with name {busCompanyName} does not exist!");
                }

                var review = new Review()
                {
                    BusCompany = company,
                    Customer = customer,
                    Grade = grade,
                    Content = content,
                    Publishing = DateTime.Now
                };

                context.Reviews.Add(review);
                context.SaveChanges();

                return $"Customer {customer.FirstName} {customer.LastName} published review for company {busCompanyName}";
            }
        }
    }
}