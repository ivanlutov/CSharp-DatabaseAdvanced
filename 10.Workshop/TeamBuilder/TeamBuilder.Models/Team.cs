namespace TeamBuilder.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Team
    {
        public int Id { get; set; }

        [MaxLength(25)]
        public string Name { get; set; }

        [MaxLength(32)]
        public string Description { get; set; }

        [MinLength(3),MaxLength(3)]
        public string Acronym { get; set; }

        public int CreatorId { get; set; }
        public User Creator { get; set; }

        public ICollection<TeamEvent> EventTeams { get; set; } = new HashSet<TeamEvent>();
        public ICollection<UserTeam> UserTeams { get; set; } = new HashSet<UserTeam>();
        public ICollection<Invitation> Invitations { get; set; } = new HashSet<Invitation>();

    }
}