namespace TeamBuilder.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public int Id { get; set; }

        [MinLength(3), MaxLength(25)]
        public string Username { get; set; }

        [MaxLength(25)]
        public string FirstName { get; set; }

        [MaxLength(25)]
        public string LastName { get; set; }

        [MinLength(6), MaxLength(60)]
        public string Password { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<Event> CreatedEvents { get; set; } = new HashSet<Event>();
        public ICollection<UserTeam> UserTeams { get; set; } = new HashSet<UserTeam>();
        public ICollection<Team> CreatedTeams { get; set; } = new HashSet<Team>();
        public ICollection<Invitation> ReceivedInvitations { get; set; } = new HashSet<Invitation>();
    }
}