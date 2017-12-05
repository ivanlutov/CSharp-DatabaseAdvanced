using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamBuilder.Models;

namespace TeamBuilder.Data.Configurations
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder
                .ToTable("Teams");

            builder
                .Property(t => t.Name)
                .HasMaxLength(25)
                .IsRequired();

            builder
                .HasIndex(t => t.Name)
                .IsUnique();

            builder
                .Property(t => t.Description)
                .HasMaxLength(32);

            builder
                .Property(t => t.Acronym)
                .HasMaxLength(3);

            builder
                .HasMany(t => t.EventTeams)
                .WithOne(et => et.Team)
                .HasForeignKey(t => t.TeamId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}