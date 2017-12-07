namespace Stations.Data
{
    using Microsoft.EntityFrameworkCore;
    using Stations.Models;
    public class StationsDbContext : DbContext
	{
	    public DbSet<CustomerCard> Cards { get; set; }
	    public DbSet<Ticket> Tickets { get; set; }
	    public DbSet<Trip> Trips { get; set; }
	    public DbSet<Train> Trains { get; set; }
	    public DbSet<Station> Stations { get; set; }
	    public DbSet<SeatingClass> SeatingClasses { get; set; }
	    public DbSet<TrainSeat> TrainSeats { get; set; }

		public StationsDbContext()
		{
		}

		public StationsDbContext(DbContextOptions options)
			: base(options)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer(Configuration.ConnectionString);
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
		    modelBuilder
		        .Entity<Station>().HasAlternateKey(s => s.Name);

		    modelBuilder
		        .Entity<Train>().HasAlternateKey(s => s.TrainNumber);

		    modelBuilder
		        .Entity<SeatingClass>().HasAlternateKey(s => new {s.Name, s.Abbreviation});

		    modelBuilder
		        .Entity<Station>()
		        .HasMany(s => s.TripsFrom)
		        .WithOne(t => t.OriginStation)
		        .HasForeignKey(t => t.OriginStationId)
		        .OnDelete(DeleteBehavior.Restrict);

		    modelBuilder
		        .Entity<Station>()
		        .HasMany(s => s.TripsTo)
		        .WithOne(t => t.DestinationStation)
		        .HasForeignKey(t => t.DestinationStationId)
		        .OnDelete(DeleteBehavior.Restrict);
		}
	}
}