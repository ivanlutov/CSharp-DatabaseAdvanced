namespace BusTicketSystem.Client.Core.Commands
{
    using System.Text;
    using BusTicketSystem.Client.Interfaces;
    using System;
    using System.Linq;
    using BusTicketSystem.Data;
    using BusTicketSystem.Models;
    using Microsoft.EntityFrameworkCore;

    public class PrintReviewsCommand : ICommand
    {
        public string Execute(string[] args)
        {
            var companyId = int.Parse(args[0]);

            using (BusTicketSystemContext context = new BusTicketSystemContext())
            {
                var company = context.BusCompanies
                    .Include(c => c.Reviews)
                        .ThenInclude(r => r.Customer)
                    .SingleOrDefault(c => c.Id == companyId);

                if (company == null)
                {
                    throw new ArgumentException("");
                }

                var sb = new StringBuilder();
                foreach (var review in company.Reviews)
                {
                    sb.AppendLine(
                        $@"{review.Id} {review.Grade} {review.Publishing} {review.Customer.FirstName} {
                                review.Customer.LastName
                            } {review.Content}");
                }

                return sb.ToString().Trim();
            }
        }
    }
}