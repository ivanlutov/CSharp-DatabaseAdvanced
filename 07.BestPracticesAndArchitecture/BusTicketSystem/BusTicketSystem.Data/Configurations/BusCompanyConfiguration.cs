namespace BusTicketSystem.Data.Configurations
{
    using BusTicketSystem.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    public class BusCompanyConfiguration : IEntityTypeConfiguration<BusCompany>
    {
        public void Configure(EntityTypeBuilder<BusCompany> builder)
        {
            builder
                .ToTable("BusCompanies");
        }
    }
}