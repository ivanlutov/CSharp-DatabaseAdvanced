using Microsoft.EntityFrameworkCore;

namespace TeamBuilder.Service
{
    using System.Linq;
    using TeamBuilder.Data;
    using TeamBuilder.Models;

    public class UserService
    {
        private readonly TeamBuilderContext context;

        public UserService(TeamBuilderContext context)
        {
            this.context = context;
        }
        public bool IsUserExistingByUsername(string username)
        {

            return context
                .Users
                .Any(u => u.Username == username && u.IsDeleted == false);
        }

        public bool IsUserExistingByUsernameAndPassword(string username, string password)
        {
            return context
                .Users
                .Any(u => u.Username == username && u.Password == password && u.IsDeleted == false);
        }

        public User Register(string username, string password, string firstName, string lastName, int age,
            Gender gender)
        {
            var user = new User()
            {
                Username = username,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                Age = age,
                Gender = gender
            };

            context.Users.Add(user);
            context.SaveChanges();

            return user;
        }

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            return context.Users.SingleOrDefault(u => u.Username == username && u.Password == password);
        }

        public void Delete(User user)
        {
            context.Attach(user);
            user.IsDeleted = true;

            context.SaveChanges();
        }

        public bool IsUserCreatorOfEvent(int userId, string eventName)
        {
            var lastEvent =
                context
                    .Events
                    .OrderByDescending(e => e.StartDate)
                    .FirstOrDefault(e => e.Name == eventName);

            return lastEvent.CreatorId == userId;
        }

        public bool IsUserCreatorOfTeam(int creatorId, string teamName)
        {
            return context
                .Users
                .Include(u => u.CreatedTeams)
                .SingleOrDefault(u => u.Id == creatorId)
                .CreatedTeams.Any(t => t.Name == teamName);
        }

        public bool IsUserCreatorOfTeam(string creatorUsername, string teamName)
        {
            return context
                .Users
                .Include(u => u.CreatedTeams)
                .SingleOrDefault(u => u.Username == creatorUsername)
                .CreatedTeams.Any(t => t.Name == teamName);
        }

        public bool IsUserInvitedFromTeam(string username, string teamName)
        {
            return
                context
                    .Users
                    .Include(u => u.ReceivedInvitations)
                        .ThenInclude(i => i.Team)
                    .Include(u => u.ReceivedInvitations)
                        .ThenInclude(i => i.InvitedUser)
                    .SingleOrDefault(u => u.Username == username)
                    .ReceivedInvitations.Any(i => i.InvitedUser.Username == username && i.Team.Name == teamName && i.IsActive == true);
        }

        public void InviteToTeam(string teamName, string username)
        {
            var user = context.Users.SingleOrDefault(u => u.Username == username);
            var team = context.Teams.SingleOrDefault(t => t.Name == teamName);

            var invitation = new Invitation
            {
                TeamId = team.Id,
                InvitedUserId = user.Id
            };

            user.ReceivedInvitations.Add(invitation);
            context.SaveChanges();
        }
    }
}