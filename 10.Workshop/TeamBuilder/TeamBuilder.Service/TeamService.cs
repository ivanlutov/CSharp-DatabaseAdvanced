namespace TeamBuilder.Service
{
    using System.Linq;
    using TeamBuilder.Data;
    using TeamBuilder.Models;
    using Microsoft.EntityFrameworkCore;

    public class TeamService
    {
        private readonly TeamBuilderContext context;
        public TeamService(TeamBuilderContext context)
        {
            this.context = context;
        }

        public void AddTeamToEvent(string teamName, string eventName)
        {
            var currentEvent =
                context
                    .Events
                    .OrderByDescending(e => e.StartDate)
                    .FirstOrDefault(e => e.Name == eventName);

            var team =
                context
                    .Teams
                    .SingleOrDefault(t => t.Name == teamName);

            var teamEvent = new TeamEvent
            {
                Team = team,
                Event = currentEvent
            };

            currentEvent.ParticipatingEventTeams.Add(teamEvent);
            team.EventTeams.Add(teamEvent);

            context
                .TeamEvents
                .Add(teamEvent);

            context
                .SaveChanges();
        }

        public bool IsAddedToEvent(string teamName, string eventName)
        {
            var eventId =
                context
                    .Events
                    .OrderByDescending(e => e.StartDate)
                    .FirstOrDefault(e => e.Name == eventName)
                    .Id;

            var teamId =
                context
                    .Teams
                    .SingleOrDefault(t => t.Name == teamName)
                    .Id;

            return
                context
                    .TeamEvents
                    .Any(te => te.TeamId == teamId && te.EventId == eventId);
        }
        public bool IsTeamExisting(string teamName)
        {
            return context.Teams.Any(t => t.Name == teamName);
        }

        public bool IsMemberOfTeam(string teamName, string username)
        {
            return context
                .UserTeams
                .Include(ut => ut.Team)
                .Include(ut => ut.User)
                .Any(ut => ut.User.Username == username && ut.Team.Name == teamName);
        }

        public void Create(string name, string acronym, string description, int creatorId)
        {
            var team = new Team
            {
                Name = name,
                Acronym = acronym,
                Description = description,
                CreatorId = creatorId
            };

            context.Teams.Add(team);
            context.SaveChanges();
        }

        public void AddUserToTeam(string teamName, string username)
        {
            var userId = context.Users.SingleOrDefault(u => u.Username == username).Id;
            var teamId = context.Teams.SingleOrDefault(t => t.Name == teamName).Id;

            var userTeam = new UserTeam()
            {
                TeamId = teamId,
                UserId = userId
            };

            context.UserTeams.Add(userTeam);
            context.SaveChanges();
        }

        public void AcceptInvitation(string teamName, string username)
        {
            var user = context
                .Users
                .Include(u => u.ReceivedInvitations)
                .SingleOrDefault(u => u.Username == username);

            var teamId = context.Teams.SingleOrDefault(t => t.Name == teamName).Id;

            user
                .ReceivedInvitations
                .SingleOrDefault(i => i.TeamId == teamId && i.InvitedUserId == user.Id)
                .IsActive = false;

            var userTeam = new UserTeam
            {
                TeamId = teamId,
                UserId = user.Id
            };

            context.UserTeams.Add(userTeam);
            context.SaveChanges();
        }

        public void DeclineInvitation(string teamName, string username)
        {
            var user = context
                .Users
                .Include(u => u.ReceivedInvitations)
                .SingleOrDefault(u => u.Username == username);

            var teamId = context.Teams.SingleOrDefault(t => t.Name == teamName).Id;

            user
                .ReceivedInvitations
                .SingleOrDefault(i => i.TeamId == teamId && i.InvitedUserId == user.Id)
                .IsActive = false;

            context.SaveChanges();
        }

        public void KickMember(string teamname, string username)
        {
            var userTeam =
                context
                .UserTeams
                .Include(ut => ut.Team)
                .Include(ut => ut.User)
                .SingleOrDefault(ut => ut.Team.Name == teamname && ut.User.Username == username);

            context.UserTeams.Remove(userTeam);
            context.SaveChanges();
        }

        public void DisbandTeam(string teamname)
        {
            var team = context
                .Teams
                .Include(t => t.Invitations)
                .SingleOrDefault(t => t.Name == teamname);

            var userTeams = context
                .UserTeams
                .Where(ut => ut.TeamId == team.Id);

            context
                .UserTeams
                .RemoveRange(userTeams);

            context
                .Invitations
                .RemoveRange(team.Invitations);

            var teamEvents = context
                .TeamEvents
                .Where(te => te.TeamId == team.Id);

            context
                .TeamEvents
                .RemoveRange(teamEvents);

            context
                .Teams
                .Remove(team);

            context.SaveChanges();
        }

        public Team GetTeamByName(string teamName)
        {
            var team =
                context
                    .Teams
                    .Include(t => t.UserTeams)
                    .ThenInclude(ut => ut.User)
                    .SingleOrDefault(t => t.Name == teamName);

            return team;
        }
    }
}