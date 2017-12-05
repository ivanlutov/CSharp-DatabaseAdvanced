using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Instagraph.Models
{
    public class User
    {
        public int Id { get; set; }

        [MaxLength(30)]
        public string Username { get; set; }

        [MaxLength(20)]
        public string Password { get; set; }

        public int ProfilePictureId { get; set; }
        public Picture ProfilePicture { get; set; }

        public ICollection<UserFollower> Followers { get; set; } = new HashSet<UserFollower>();
        public ICollection<UserFollower> UsersFollowing { get; set; } = new HashSet<UserFollower>();
        public ICollection<Post> Posts { get; set; } = new HashSet<Post>();
        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
    }
}