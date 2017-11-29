using System;
using System.Linq;
using BusTicketSystem.Data;
using BusTicketSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BusTicketSystem.Client.Core.Commands
{
    using BusTicketSystem.Client.Interfaces;

    public class ChangeTripStatusCommand : ICommand
    {
        public string Execute(string[] args)
        {
            var tripId = int.Parse(args[0]);
            var newStatusToString = args[1];

            using (BusTicketSystemContext context = new BusTicketSystemContext())
            {
                var trip = context
                    .Trips
                    .Include(t => t.OriginBusStation)
                        .ThenInclude(bs => bs.Town)
                    .Include(t => t.DestinationBusStation)
                        .ThenInclude(bs => bs.Town)
                    .SingleOrDefault(t => t.Id == tripId);
                if (trip == null)
                {
                    throw new ArgumentException($"Trip with {tripId} does not exist!");
                }

                object status;
                Enum.TryParse(typeof(Status), newStatusToString, out status);

                if (status == null)
                {
                    throw new ArgumentException("Invalid status!");
                }

                var oldStatus = trip.Status;
                var newStatus = (Status) status;
                trip.Status = newStatus;

                bool arrivedStatus = false;

                ArrivedTrip arrivedTrip = null;
                if (newStatus == Status.Arrived)
                {
                    arrivedStatus = true;
                    var passengersCount = context
                        .Tickets
                        .Include(t => t.Trip)
                        .Count(t => t.Trip.Id == trip.Id);

                    arrivedTrip = new ArrivedTrip
                    {
                        OriginBusStation = trip.OriginBusStation,
                        DestinationBusStation = trip.DestinationBusStation,
                        ArrivalTime = trip.ArrivalTime,
                        PassengersCount = passengersCount
                    };

                    context.ArrivedTrips.Add(arrivedTrip);
                }

                context.SaveChanges();
                var result =
                    $@"Trip from {trip.OriginBusStation.Town.Name} to {trip.DestinationBusStation.Town.Name} on {
                            trip.DepartureTime
                        } Status changed from {oldStatus} to {trip.Status}";

                if (arrivedStatus)
                {
                    result += $" - {arrivedTrip.PassengersCount} passengers arrived at {arrivedTrip.DestinationBusStation.Town.Name} from {arrivedTrip.OriginBusStation.Town.Name}";
                }

                return result;
            }
        }
    }
}