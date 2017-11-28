using System;
using System.Collections.Generic;

namespace BusTicketSystem.Models
{
    public class BusCompany
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Nationality { get; set; }
        public double Rating { get; set; }

        public ICollection<Trip> Trips { get; set; } = new HashSet<Trip>();
        public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();

    }
}