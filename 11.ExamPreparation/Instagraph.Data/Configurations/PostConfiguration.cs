using Instagraph.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Instagraph.Data.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder
                .ToTable("Posts");

            builder
                .HasKey(p => p.Id);

            builder
                .Property(p => p.Caption)
                .IsRequired();

            builder
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(p => p.Picture)
                .WithMany(p => p.Posts)
                .HasForeignKey(p => p.PictureId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(p => p.Comments)
                .WithOne(c => c.Post)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}