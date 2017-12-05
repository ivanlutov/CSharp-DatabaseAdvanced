namespace TeamBuilder.App.Core.Commands
{
    using System;
    using TeamBuilder.App.Utilities;
    using TeamBuilder.Service;
    public class AcceptInviteCommand : ICommand
    {
        private readonly UserService userService;
        private readonly TeamService teamService;

        public AcceptInviteCommand(UserService userService, TeamService teamService)
        {
            this.userService = userService;
            this.teamService = teamService;
        }

        public string Execute(params string[] args)
        {
            Check.CheckLength(1, args);

            if (Session.CurrentUser == null)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LoginFirst);
            }

            var teamName = args[0];

            if (!teamService.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            var username = Session.CurrentUser.Username;
            if (!userService.IsUserInvitedFromTeam(username, teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.InviteNotFound, teamName));
            }

            teamService.AcceptInvitation(teamName, username);

            return String.Format(Constants.Succes.UserAddedToTeam, username, teamName);

        }
    }
}