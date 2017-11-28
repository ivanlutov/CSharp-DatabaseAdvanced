namespace BusTicketSystem.Data.Configurations
{
    using BusTicketSystem.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder
                .ToTable("Reviews");

            builder
                .HasOne(r => r.BusCompany)
                .WithMany(bs => bs.Reviews)
                .HasForeignKey(r => r.BusCompanyId);
        }
    }
}