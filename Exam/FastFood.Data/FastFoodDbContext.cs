namespace FastFood.Data
{
    using FastFood.Models;
    using Microsoft.EntityFrameworkCore;
    public class FastFoodDbContext : DbContext
	{
	    public DbSet<Category> Categories { get; set; }
	    public DbSet<Employee> Employees { get; set; }
	    public DbSet<Item> Items { get; set; }
	    public DbSet<Order> Orders { get; set; }
	    public DbSet<OrderItem> OrderItems { get; set; }
	    public DbSet<Position> Positions { get; set; }

		public FastFoodDbContext()
		{
		}

		public FastFoodDbContext(DbContextOptions options)
			: base(options)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder builder)
		{
			if (!builder.IsConfigured)
			{
				builder.UseSqlServer(Configuration.ConnectionString);
			}
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
		    builder.Entity<Order>()
		        .Ignore(o => o.TotalPrice);

		    builder.Entity<Position>()
		        .HasAlternateKey(p => p.Name);

		    builder
		        .Entity<Item>()
		        .HasAlternateKey(i => i.Name);

		    builder
		        .Entity<OrderItem>()
		        .HasKey(or => new {or.OrderId, or.ItemId});

		}
	}
}