namespace TeamBuilder.App.Core.Commands
{
    using System;
    using TeamBuilder.App.Utilities;
    using TeamBuilder.Service;

    public class KickMemberCommand : ICommand
    {
        private readonly UserService userService;
        private readonly TeamService teamService;

        public KickMemberCommand(UserService userService, TeamService teamService)
        {
            this.userService = userService;
            this.teamService = teamService;
        }

        public string Execute(params string[] args)
        {
            Check.CheckLength(2, args);

            if (Session.CurrentUser == null)
            {
                throw new ArgumentException(Constants.ErrorMessages.LoginFirst);
            }

            var teamname = args[0];
            if (!teamService.IsTeamExisting(teamname))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamname));
            }

            var username = args[1];
            if (!userService.IsUserExistingByUsername(username))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.UserNotFound, username));
            }

            var loggedUserId = Session.CurrentUser.Id;
            var isLoggedUserCreatorOfTeam = userService.IsUserCreatorOfTeam(loggedUserId, teamname);
            if (!isLoggedUserCreatorOfTeam)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            var isUserToKickCreatorOfTeam = userService.IsUserCreatorOfTeam(username, teamname);
            if (isUserToKickCreatorOfTeam)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowedDisbandTeam);
            }

            var isUserMemberOfTeam = teamService.IsMemberOfTeam(teamname, username);
            if (!isUserMemberOfTeam)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.NotMemberOfTeam, username, teamname));
            }

            teamService.KickMember(teamname, username);

            return string.Format(Constants.Succes.KickedMember, username, teamname);
        }
    }
}