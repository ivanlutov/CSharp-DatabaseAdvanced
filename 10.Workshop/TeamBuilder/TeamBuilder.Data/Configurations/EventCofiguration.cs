namespace TeamBuilder.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TeamBuilder.Models;

    public class EventCofiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder
                .ToTable("Events");

            builder
                .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(25);

            builder
                .Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(250);

            builder
                .HasMany(e => e.ParticipatingEventTeams)
                .WithOne(et => et.Event)
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}