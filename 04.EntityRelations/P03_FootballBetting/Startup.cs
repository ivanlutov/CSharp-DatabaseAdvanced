namespace P03_FootballBetting
{
    using Microsoft.EntityFrameworkCore;
    using Data;
    public class Startup
    {
        public static void Main()
        {
            var context = new FootballBettingContext();
            context.Database.EnsureDeleted();
            context.Database.Migrate();
        }
    }
}
