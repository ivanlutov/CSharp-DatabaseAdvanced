using BusTicketSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusTicketSystem.Data.Configurations
{
    public class ArrivedTripConfiguration : IEntityTypeConfiguration<ArrivedTrip>
    {
        public void Configure(EntityTypeBuilder<ArrivedTrip> builder)
        {
            builder
                .ToTable("ArrivedTrips");
        }
    }
}