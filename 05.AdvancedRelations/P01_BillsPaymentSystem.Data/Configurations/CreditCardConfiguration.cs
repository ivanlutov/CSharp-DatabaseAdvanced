namespace P01_BillsPaymentSystem.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;
    public class CreditCardConfiguration : IEntityTypeConfiguration<CreditCard>
    {
        public void Configure(EntityTypeBuilder<CreditCard> builder)
        {
            builder
                .HasKey(c => c.CreditCardId);

            builder
                .ToTable("CreditCards");

            builder
                .Property(c => c.Limit)
                .IsRequired(true);

            builder
                .Property(c => c.MoneyOwed)
                .IsRequired(true);

            builder
                .Property(c => c.ExpirationDate)
                .IsRequired(true);

            builder
                .Ignore(c => c.LimitLeft);

            builder
                .Ignore(c => c.PaymentMethodId);
        }
    }
}