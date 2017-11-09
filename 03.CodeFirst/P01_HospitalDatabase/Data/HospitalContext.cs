namespace P01_HospitalDatabase.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;
    public class HospitalContext : DbContext
    {
        public DbSet<Diagnose> Diagnoses { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Visitation> Visitations { get; set; }
        public DbSet<PatientMedicament> Prescriptions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Patient>()
                .HasKey(p => p.PatientId);

            modelBuilder.Entity<Patient>(p =>
            {
                p.Property(pr => pr.FirstName).HasMaxLength(50).IsUnicode(true);
                p.Property(pr => pr.LastName).HasMaxLength(50).IsUnicode(true);
                p.Property(pr => pr.Address).HasMaxLength(250).IsUnicode(true);
                p.Property(pr => pr.Email).HasMaxLength(80).IsUnicode(false);
            });

            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Visitations)
                .WithOne(v => v.Patient)
                .HasForeignKey(v => v.PatientId);

            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Diagnoses)
                .WithOne(d => d.Patient)
                .HasForeignKey(d => d.PatientId);

            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Prescriptions)
                .WithOne(pm => pm.Patient)
                .HasForeignKey(pm => pm.PatientId);

            modelBuilder.Entity<Visitation>()
                .HasKey(v => v.VisitationId);

            modelBuilder.Entity<Visitation>(v =>
            {
                v.Property(p => p.Comments).HasMaxLength(250).IsUnicode(true);
            });

            modelBuilder.Entity<Diagnose>()
                .HasKey(d => d.DiagnoseId);

            modelBuilder.Entity<Diagnose>(d =>
                {
                    d.Property(p => p.Name).HasMaxLength(50).IsUnicode(true);
                    d.Property(p => p.Comments).HasMaxLength(250).IsUnicode(true);
                });

            modelBuilder.Entity<Medicament>()
                .HasKey(m => m.MedicamentId);

            modelBuilder.Entity<Medicament>(m =>
            {
                m.Property(p => p.Name).HasMaxLength(50).IsUnicode(true);
            });

            modelBuilder.Entity<Medicament>()
                .HasMany(m => m.Prescriptions)
                .WithOne(pm => pm.Medicament)
                .HasForeignKey(pm => pm.MedicamentId);

            modelBuilder.Entity<PatientMedicament>()
                .ToTable("PatientMedicament");

            modelBuilder.Entity<PatientMedicament>()
                .HasKey(pm => new
                {
                    pm.PatientId,
                    pm.MedicamentId
                });

            modelBuilder.Entity<PatientMedicament>()
                .HasOne(pm => pm.Patient)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(pm => pm.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PatientMedicament>()
                .HasOne(pm => pm.Medicament)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(pm => pm.MedicamentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Doctor>()
                .HasKey(d => d.DoctorId);

            modelBuilder.Entity<Doctor>(d =>
            {
                d.Property(p => p.Name).HasMaxLength(100).IsUnicode(true);
                d.Property(p => p.Specialty).HasMaxLength(100).IsUnicode(true);
            });

            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.Visitations)
                .WithOne(v => v.Doctor)
                .HasForeignKey(v => v.DoctorId);
        }
    }
}