namespace P03_FootballBetting.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder
                .ToTable("Games");

            builder
                .HasOne(t => t.AwayTeam)
                .WithMany(g => g.AwayGames)
                .HasForeignKey(t => t.AwayTeamId);

            builder
                .HasOne(t => t.HomeTeam)
                .WithMany(g => g.HomeGames)
                .HasForeignKey(t => t.HomeTeamId);
        }
    }
}