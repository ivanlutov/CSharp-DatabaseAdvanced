namespace EmployeesSystem.Data.Configurations
{
    using EmployeesSystem.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder
                .ToTable("Employees");

            builder
                .Property(e => e.FirstName)
                .IsRequired();

            builder
                .Property(e => e.LastName)
                .IsRequired();

            builder
                .Property(e => e.Salary)
                .IsRequired();

            builder
                .HasOne(e => e.Manager)
                .WithMany(m => m.Subordinates)
                .HasForeignKey(e => e.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}