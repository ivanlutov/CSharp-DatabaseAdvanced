namespace P01_StudentSystem
{
    using Microsoft.EntityFrameworkCore;
    using Data;
    public class Startup
    {
        public static void Main()
        {
            var context = new StudentSystemContext();

            context.Database.EnsureDeleted();

            context.Database.Migrate();
        }
    }
}
