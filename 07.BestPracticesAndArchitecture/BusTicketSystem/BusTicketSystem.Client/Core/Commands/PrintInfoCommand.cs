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

                var busStations = context
                    .BusStations
                    .Where(bs => bs.Id == busStationId)
                    .Select(bs => new
                    {
                        BusStationName = bs.Name,
                        BusStationTownName = bs.Town.Name,
                        Arrivals = bs.OriginTrips.Select(t => new
                        {
                            OriginaStationName = t.OriginBusStation,
                            ArriveAt = t.ArrivalTime,
                            Status = t.Status
                        })
                        .ToList(),
                        Departures = bs.DestinationTrips.Select(d => new
                        {
                            DestinationStationName = d.DestinationBusStation.Name,
                            DepartAt = d.DepartureTime,
                            Status = d.Status
                        })
                        .ToList()
                    })
                    .ToList();

                var sb = new StringBuilder();

                foreach (var busStation in busStations)
                {
                    sb.AppendLine($"{busStation.BusStationName}, {busStation.BusStationTownName}");
                    sb.AppendLine("Arrivals:");
                    foreach (var trip in busStation.Arrivals)
                    {
                        sb.AppendLine($"From: {trip.OriginaStationName} | Arrive at: {trip.ArriveAt.Hour}:{trip.ArriveAt.Minute} | Status: {trip.Status}");
                    }

                    sb.AppendLine("Departures:");
                    foreach (var trip in busStation.Departures)
                    {
                        sb.AppendLine($"From: {trip.DestinationStationName} | Depart at: {trip.DepartAt.Hour}:{trip.DepartAt.Minute} | Status: {trip.Status}");
                    }
                }
                //sb.AppendLine($"{busStation.}, {busStation.Town.Name}");
                //sb.AppendLine("Arrivals:");
                //foreach (var trip in busStation.OriginTrips)
                //{
                //    sb.AppendLine($"From: {trip.OriginBusStation.Town.Name} | Arrive at: {trip.ArrivalTime.Hour}:{trip.ArrivalTime.Minute} | Status: {trip.Status}");
                //}

                //sb.AppendLine("Departures:");
                //foreach (var trip in busStation.DestinationTrips)
                //{
                //    sb.AppendLine($"From: {trip.DestinationBusStation.Town.Name} | Depart at: {trip.DepartureTime.Hour}:{trip.DepartureTime.Minute} | Status: {trip.Status}");
                //}

                return sb.ToString().Trim();
            }
        }
    }
}