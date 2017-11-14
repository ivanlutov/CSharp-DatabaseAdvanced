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

            //builder
            //    .HasMany(g => g.Bets)
            //    .WithOne(b => b.Game)
            //    .HasForeignKey(b => b.GameId);
        }
    }
}