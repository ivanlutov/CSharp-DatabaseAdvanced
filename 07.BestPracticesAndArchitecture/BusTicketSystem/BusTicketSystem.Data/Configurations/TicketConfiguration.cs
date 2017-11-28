namespace BusTicketSystem.Data.Configurations
{
    using BusTicketSystem.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder
                .ToTable("Tickets");

            builder
                .HasOne(tic => tic.Customer)
                .WithOne(c => c.Ticket)
                .HasForeignKey<Ticket>(tic => tic.CustomerId);
        }
    }
}