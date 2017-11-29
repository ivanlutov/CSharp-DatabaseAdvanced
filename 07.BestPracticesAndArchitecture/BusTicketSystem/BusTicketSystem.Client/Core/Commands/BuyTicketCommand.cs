namespace BusTicketSystem.Client.Core.Commands
{
    using System;
    using System.Linq;
    using BusTicketSystem.Data;
    using BusTicketSystem.Models;
    using Microsoft.EntityFrameworkCore;
    using BusTicketSystem.Client.Interfaces;

    public class BuyTicketCommand : ICommand
    {
        public string Execute(string[] args)
        {
            var customerId = int.Parse(args[0]);
            var tripId = int.Parse(args[1]);
            var price = decimal.Parse(args[2]);
            var seat = args[3];

            using (BusTicketSystemContext context = new BusTicketSystemContext())
            {
                var customer = context.Customers.Include(c => c.BankAccount).SingleOrDefault(c => c.Id == customerId);
                if (customer == null)
                {
                    throw new ArgumentException($"Customer with {customerId} does not exist!");
                }

                var trip = context.Trips.SingleOrDefault(t => t.Id == tripId);
                if (trip == null)
                {
                    throw new ArgumentException($"Trip with {tripId} does not exist!");
                }

                if (customer.BankAccount == null)
                {
                    throw new ArgumentException($"Customer {customer.FirstName} {customer.LastName} dont have bank account!");
                }

                if (!customer.BankAccount.IsEnoughMoney(price))
                {
                    throw new ArgumentException($"Customer {customer.FirstName} {customer.LastName} dont have enought money!");
                }

                customer.BankAccount.Withdraw(price);

                var ticket = new Ticket()
                {
                   Customer = customer,
                   Price = price,
                   Seat = seat,
                   Trip = trip
                };
                context.Tickets.Add(ticket);
                customer.Ticket = ticket;

                context.SaveChanges();

                return $"Customer {customer.FirstName} {customer.LastName} bought ticket for trip {trip.Id} for ${price:F2} on seat {seat}";
            }
        }
    }
}