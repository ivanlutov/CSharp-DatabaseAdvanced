using System.Collections.Generic;
using BusTicketSystem.Client.Core;
using BusTicketSystem.Client.Interfaces;
using BusTicketSystem.Data;
using BusTicketSystem.Models;
using Microsoft.EntityFrameworkCore.Query.Expressions;

namespace BusTicketSystem.Client
{
    using System;
    public class Application
    {
        public static void Main(string[] args)
        {
            //ResetDatabaseAndSeed();
            ICommandDispatcher dispatcher = new CommandDispatcher();
            IWriter writer = new ConsoleWriter();
            IReader reader = new ConsoleReader();
            IEngine engine = new Engine(dispatcher,writer,reader);
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

                db.Towns.AddRange(townSofia, townPlovdiv, townBurgas, townSvishtov, townTurnovo, townVarna);
                db.SaveChanges();

                var busCompany = new BusCompany()
                {
                    Name = "Company",
                    Nationality = "BG",
                    Rating = 5
                };


                //var busStation = new BusStation()
                //{
                //    Name = "Sofia",
                //    Town = townSofia,
                //    OriginTrips = new List<Trip>()
                //    {
                       
                //    }
                //};
               
                //var arrTrip1 = new Trip()
                //{
                //    OriginBusStation = new BusStation()
                //    {
                //        Town = townBurgas
                //    },
                //    DestinationBusStation = new BusStation()
                //    {
                //        Town = townSofia
                //    },
                //    ArrivalTime = new DateTime(2017,01,01,14,30,00),
                //    Status = Status.Departed,
                //    BusCompany = busCompany
                //};

                //var arrTrip2 = new Trip()
                //{
                //    OriginBusStation = new BusStation()
                //    {
                //        Town = townSvishtov
                //    },
                //    DestinationBusStation = new BusStation()
                //    {
                //        Town = townSofia
                //    },
                //    ArrivalTime = new DateTime(2017, 01, 01, 7, 30, 00),
                //    Status = Status.Arrived,
                //    BusCompany = busCompany
                //};

                //var arrTrip3 = new Trip()
                //{
                //    OriginBusStation = new BusStation()
                //    {
                //        Town = townTurnovo
                //    },
                //    DestinationBusStation = new BusStation()
                //    {
                //        Town = townSofia
                //    },
                //    ArrivalTime = new DateTime(2017, 01, 01, 14, 30, 00),
                //    Status = Status.Departed,
                //    BusCompany = busCompany
                //};

                //var destTrip1 = new Trip()
                //{
                //    DestinationBusStation = new BusStation()
                //    {
                //        Name = "Varna",
                //        Town = townVarna,
                        
                //    },
                //    OriginBusStation = new BusStation()
                //    {
                //        Town = townSofia
                //    },
                //    ArrivalTime = new DateTime(2017, 01, 01, 14, 40, 00),
                //    Status = Status.Delayed,
                //    BusCompany = busCompany
                //};

                //var destTrip2 = new Trip()
                //{
                //    DestinationBusStation = new BusStation()
                //    {
                //        Town = townPlovdiv
                //    },
                //    OriginBusStation = new BusStation()
                //    {
                //        Town = townSofia
                //    },
                //    ArrivalTime = new DateTime(2017, 01, 01, 15, 30, 00),
                //    Status = Status.Cancelled,
                //    BusCompany = busCompany
                //};

                //db.Trips.AddRange(arrTrip1, arrTrip2, arrTrip3, destTrip1, destTrip2);
                db.SaveChanges();
            }
        }
    }
}
