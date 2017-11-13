namespace P01_StudentSystem.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder
                .Property(p => p.Name)
                .HasMaxLength(80)
                .IsUnicode(true);

            builder
                .Property(p => p.Description)
                .IsUnicode(true)
                .IsRequired(false);

            builder
                .HasMany(c => c.Resources)
                .WithOne(r => r.Course)
                .HasForeignKey(r => r.CourseId);

            builder
                .HasMany(c => c.HomeworkSubmissions)
                .WithOne(hm => hm.Course)
                .HasForeignKey(hm => hm.CourseId);

            builder
                .ToTable("Courses");
        }
    }
}