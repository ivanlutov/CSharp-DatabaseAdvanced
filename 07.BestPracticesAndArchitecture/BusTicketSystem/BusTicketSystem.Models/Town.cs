namespace BusTicketSystem.Models
{
    using System.Collections.Generic;

    public class Town
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }

        public ICollection<Customer> CustomerHomeTowns { get; set; } = new HashSet<Customer>();
        public ICollection<BusStation> BusStations { get; set; } = new HashSet<BusStation>();
    }
}