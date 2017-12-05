using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using TeamBuilder.Data.Configurations;
using TeamBuilder.Models;

namespace TeamBuilder.Data
{
    public class TeamBuilderContext : DbContext
    {
        public TeamBuilderContext()
        {
            
        }

        public TeamBuilderContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamEvent> TeamEvents { get; set; }
        public DbSet<UserTeam> UserTeams { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionCofiguration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EventCofiguration());
            modelBuilder.ApplyConfiguration(new InvitationConfiguration());
            modelBuilder.ApplyConfiguration(new TeamConfiguration());
            modelBuilder.ApplyConfiguration(new TeamEventConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserTeamConfiguration());
        }
    }
}