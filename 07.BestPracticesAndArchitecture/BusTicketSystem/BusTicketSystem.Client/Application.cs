

namespace BusTicketSystem.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BusTicketSystem.Client.Core;
    using BusTicketSystem.Client.Interfaces;
    using BusTicketSystem.Data;
    using BusTicketSystem.Models;
    using Microsoft.EntityFrameworkCore.Query.Expressions;
    public class Application
    {
        public static void Main(string[] args)
        {
            //ResetDatabaseAndSeed();
            ICommandDispatcher dispatcher = new CommandDispatcher();
            IWriter writer = new ConsoleWriter();
            IReader reader = new ConsoleReader();
            IEngine engine = new Engine(dispatcher, writer, reader);
            engine.Run();
        }

        private static void ResetDatabaseAndSeed()
        {
            using (var db = new BusTicketSystemContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                var townSofia = new Town()
                {
                    Name = "Sofia",
                    Country = "Bulgaria"
                };

                var townBurgas = new Town()
                {
                    Name = "Burgas",
                    Country = "Bulgaria"
                };

                var townSvishtov = new Town()
                {
                    Name = "Svishtov",
                    Country = "Bulgaria"
                };

                var townTurnovo = new Town()
                {
                    Name = "V.Turnovo",
                    Country = "Bulgaria"
                };

                var townVarna = new Town()
                {
                    Name = "Varna",
                    Country = "Bulgaria"
                };

                var townPlovdiv = new Town()
                {
                    Name = "Plovdiv",
                    Country = "Bulgaria"
                };

                db.Towns.AddRange(townPlovdiv, townBurgas, townSofia, townSvishtov, townTurnovo, townVarna);
                db.SaveChanges();

                var busCompany = new BusCompany()
                {
                    Name = "Trans5",
                    Nationality = "BG",
                    Rating = 9,
                };
                db.BusCompanies.Add(busCompany);
                db.SaveChanges();

                var busStation1 = new BusStation()
                {
                    Name = "Burgas",
                    Town = townBurgas,
                };

                var busStation2 = new BusStation()
                {
                    Name = "Plovdiv",
                    Town = townPlovdiv,
                };

                db.BusStations.AddRange(busStation1, busStation2);
                db.SaveChanges();

                var trip1 = new Trip()
                {
                    OriginBusStation = busStation1,
                    DestinationBusStation = busStation2,
                    Status = Status.Delayed,
                    BusCompany = busCompany,
                    ArrivalTime = DateTime.Now,
                    DepartureTime = DateTime.Now,
                };

                var trip2 = new Trip()
                {
                    OriginBusStation = busStation2,
                    DestinationBusStation = busStation1,
                    Status = Status.Departed,
                    BusCompany = busCompany,
                    ArrivalTime = DateTime.Now,
                    DepartureTime = DateTime.Now,
                };

                db.Trips.AddRange(trip1, trip2);
                db.SaveChanges();

                var bankAccount = new BankAccount()
                {
                    AccountNumber = "ABCD123",
                    Balance = 5000
                };
                db.BankAccounts.Add(bankAccount);
                db.SaveChanges();

                var customer = new Customer()
                {
                    FirstName = "Ivan",
                    LastName = "Lutov",
                    HomeTown = townSofia,
                    Gender = Gender.Male,
                    DateOfBirth = new DateTime(1990, 08, 11),
                    BankAccount = bankAccount
                };

                db.Customers.Add(customer);
                db.SaveChanges();

                var ticket = new Ticket()
                {
                    Price = 500,
                    Seat = "A1",
                    Trip = trip1,
                    Customer = customer
                };

                db.Tickets.Add(ticket);
                db.SaveChanges();
            }
        }
    }
}
