namespace BusTicketSystem.Data.Configurations
{
    using BusTicketSystem.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder
                .ToTable("Customers");

            builder
                .HasOne(c => c.HomeTown)
                .WithMany(t => t.CustomerHomeTowns)
                .HasForeignKey(c => c.HomeTownId);

            builder
                .HasMany(c => c.Reviews)
                .WithOne(r => r.Customer)
                .HasForeignKey(r => r.CustomerId);
        }
    }
}