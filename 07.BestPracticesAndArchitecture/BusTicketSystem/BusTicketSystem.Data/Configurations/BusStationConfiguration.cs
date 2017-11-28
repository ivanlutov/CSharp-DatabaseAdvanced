namespace BusTicketSystem.Data.Configurations
{
    using BusTicketSystem.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    public class BusStationConfiguration : IEntityTypeConfiguration<BusStation>
    {
        public void Configure(EntityTypeBuilder<BusStation> builder)
        {
            builder
                .ToTable("BusStations");

            builder
                .HasOne(bs => bs.Town)
                .WithMany(t => t.BusStations)
                .HasForeignKey(bs => bs.TownId);
        }
    }
}