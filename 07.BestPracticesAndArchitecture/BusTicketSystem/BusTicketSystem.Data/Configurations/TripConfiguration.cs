namespace BusTicketSystem.Data.Configurations
{
    using BusTicketSystem.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    public class TripConfiguration : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {
            builder
                .ToTable("Trips");

            builder
                .HasOne(t => t.OriginBusStation)
                .WithMany(bs => bs.OriginTrips)
                .HasForeignKey(t => t.OriginBusStationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(t => t.DestinationBusStation)
                .WithMany(bs => bs.DestinationTrips)
                .HasForeignKey(t => t.DestinationBusStationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(t => t.BusCompany)
                .WithMany(bc => bc.Trips)
                .HasForeignKey(t => t.BusCompanyId);
            //.HasForeignKey(t => t.BusCompanyId);
        }
    }
}