namespace EmployeesSystem.Data
{
    using EmployeesSystem.Data.Configurations;
    using Microsoft.EntityFrameworkCore;
    using EmployeesSystem.Models;

    public class EmployeeSystemContext : DbContext
    {
        public EmployeeSystemContext()
        {
            
        }
        public EmployeeSystemContext(DbContextOptions options) 
            : base(options)
        {
            
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        }
    }
}