﻿namespace BusTicketSystem.Models
{
    using System;
    using System.Collections.Generic;

    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }

        public int? HomeTownId { get; set; }
        public Town HomeTown { get; set; }

        public BankAccount BankAccount { get; set; }

        public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();

        public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}