using Instagraph.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Instagraph.Data.Configurations
{
    public class PictureCofiguration : IEntityTypeConfiguration<Picture>
    {
        public void Configure(EntityTypeBuilder<Picture> builder)
        {
            builder
                .ToTable("Pictures");

            builder
                .HasKey(p => p.Id);

            builder
                .Property(p => p.Path)
                .IsRequired();

            builder
                .Property(p => p.Size)
                .IsRequired();

            builder
                .HasMany(p => p.Users)
                .WithOne(u => u.ProfilePicture)
                .HasForeignKey(u => u.ProfilePictureId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(p => p.Posts)
                .WithOne(p => p.Picture)
                .HasForeignKey(p => p.PictureId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}