namespace BusTicketSystem.Data.Configurations
{
    using BusTicketSystem.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder
                .ToTable("BankAccounts");

            builder
                .HasOne(b => b.Customer)
                .WithOne(c => c.BankAccount)
                .HasForeignKey<BankAccount>(b => b.CustomerId);
        }
    }
}