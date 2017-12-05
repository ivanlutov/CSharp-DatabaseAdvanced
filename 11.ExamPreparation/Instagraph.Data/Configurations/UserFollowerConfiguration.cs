using Instagraph.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Instagraph.Data.Configurations
{
    public class UserFollowerConfiguration : IEntityTypeConfiguration<UserFollower>
    {
        public void Configure(EntityTypeBuilder<UserFollower> builder)
        {
            builder
                .ToTable("UsersFollowers");

            builder
                .HasKey(uf => new {uf.UserId, uf.FollowerId});

            builder
                .HasOne(uf => uf.User)
                .WithMany(u => u.Followers)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(uf => uf.Follower)
                .WithMany(u => u.UsersFollowing)
                .HasForeignKey(uf => uf.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}