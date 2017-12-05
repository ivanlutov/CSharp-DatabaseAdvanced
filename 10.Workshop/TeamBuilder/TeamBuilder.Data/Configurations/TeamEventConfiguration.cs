using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamBuilder.Models;

namespace TeamBuilder.Data.Configurations
{
    public class TeamEventConfiguration : IEntityTypeConfiguration<TeamEvent>
    {
        public void Configure(EntityTypeBuilder<TeamEvent> builder)
        {
            builder
                .ToTable("EventTeams");

            builder
                .HasKey(et => new {et.EventId, et.TeamId});

            builder
                .HasOne(et => et.Event)
                .WithMany(e => e.ParticipatingEventTeams)
                .HasForeignKey(et => et.EventId);

            builder
                .HasOne(et => et.Team)
                .WithMany(t => t.EventTeams)
                .HasForeignKey(et => et.TeamId);
        }
    }
}