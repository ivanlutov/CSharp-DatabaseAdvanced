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
        //change-trip-status {Trip Id} {New Status}
        public string Execute(string[] args)
        {
            var tripId = int.Parse(args[0]);
            var newStatus = args[1];

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
                Enum.TryParse(typeof(Status), newStatus, out status);

                if (status == null)
                {
                    throw new ArgumentException("Invalid status!");
                }

                var oldStatus = trip.Status;
                trip.Status = (Status)status;
                context.SaveChanges();

                return $@"Trip from {trip.OriginBusStation.Town.Name} to {trip.DestinationBusStation.Town.Name} on {
                        trip.DepartureTime
                    } Status changed from {oldStatus} to {trip.Status}";
            }
        }
    }
}