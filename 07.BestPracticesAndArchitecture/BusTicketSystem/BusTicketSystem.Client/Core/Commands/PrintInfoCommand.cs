using BusTicketSystem.Models;

namespace BusTicketSystem.Client.Core.Commands
{
    using System;
    using System.Linq;
    using System.Text;
    using BusTicketSystem.Client.Interfaces;
    using BusTicketSystem.Data;
    using Microsoft.EntityFrameworkCore;

    public class PrintInfoCommand : ICommand
    {
        public string Execute(string[] args)
        {
            var busStationId = int.Parse(args[0]);

            using (BusTicketSystemContext context = new BusTicketSystemContext())
            {
                if (context.BusStations.SingleOrDefault(bs => bs.Id == busStationId) == null)
                {
                    throw new ArgumentException("Bus station doen not exist!");
                }

                var busStation = context
                    .BusStations
                    .Include(b => b.Town)
                    .Include(b => b.OriginTrips)
                        .ThenInclude(o => o.OriginBusStation)
                    .Include(b => b.DestinationTrips)
                        .ThenInclude(d => d.DestinationBusStation)
                    .SingleOrDefault(bs => bs.Id == busStationId);

                var sb = new StringBuilder();

                sb.AppendLine($"{busStation.Name}, {busStation.Town.Name}");
                sb.AppendLine("Arrivals:");
                foreach (var trip in busStation.OriginTrips)
                {
                    sb.AppendLine($"From {trip.OriginBusStation.Name} | Arrive at: {trip.ArrivalTime} | Status: {trip.Status}");
                }

                sb.AppendLine("Departures:");
                foreach (var trip in busStation.DestinationTrips)
                {
                    sb.AppendLine($"To {trip.DestinationBusStation.Name} | Arrive at: {trip.DepartureTime} | Status: {trip.Status}");
                }
               
                return sb.ToString().Trim();
            }
        }
    }
}