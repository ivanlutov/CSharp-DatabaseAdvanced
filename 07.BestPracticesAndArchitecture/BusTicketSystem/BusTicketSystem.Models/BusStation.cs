namespace BusTicketSystem.Models
{
    using System.Collections.Generic;

    public class BusStation
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int TownId { get; set; }
        public Town Town { get; set; }

        public ICollection<Trip> OriginTrips { get; set; } = new HashSet<Trip>();

        public ICollection<Trip> DestinationTrips { get; set; } = new HashSet<Trip>();

    }
}