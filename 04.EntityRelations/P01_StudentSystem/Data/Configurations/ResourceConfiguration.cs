namespace P01_StudentSystem.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;
    public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder
                .Property(p => p.Name)
                .HasMaxLength(50)
                .IsUnicode(true);

            builder
                .Property(p => p.Url)
                .IsUnicode(false);

            builder
                .ToTable("Resources");
        }
    }
}