namespace BusTicketSystem.Models
{
    using System;

    public class ArrivedTrip
    {
        public int Id { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int PassengersCount { get; set; }

        public int OriginBusStationId { get; set; }
        public BusStation OriginBusStation { get; set; }

        public int DestinationBusStationId { get; set; }
        public BusStation DestinationBusStation { get; set; }
    }
}