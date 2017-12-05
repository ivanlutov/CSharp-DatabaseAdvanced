namespace TeamBuilder.App.Core.Commands
{
    using System;
    using TeamBuilder.App.Utilities;
    using TeamBuilder.Service;
    public class DisbandCommand : ICommand
    {
        private readonly UserService userService;
        private readonly TeamService teamService;

        public DisbandCommand(UserService userService, TeamService teamService)
        {
            this.userService = userService;
            this.teamService = teamService;
        }

        public string Execute(params string[] args)
        {
            Check.CheckLength(1 , args);

            if (Session.CurrentUser == null)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LoginFirst);
            }

            var teamname = args[0];
            if (!teamService.IsTeamExisting(teamname))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamname));
            }

            var loggedUserId = Session.CurrentUser.Id;
            if (!userService.IsUserCreatorOfTeam(loggedUserId, teamname))
            {
                throw new ArgumentException(Constants.ErrorMessages.NotAllowed);
            }

            teamService.DisbandTeam(teamname);

            return string.Format(Constants.Succes.DisbandTeam, teamname);
        }
    }
}