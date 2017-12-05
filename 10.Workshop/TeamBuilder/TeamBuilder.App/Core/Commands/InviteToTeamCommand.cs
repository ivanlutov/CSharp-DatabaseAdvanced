namespace TeamBuilder.App.Core.Commands
{
    using System;
    using TeamBuilder.App.Utilities;
    using TeamBuilder.Service;
    public class InviteToTeamCommand : ICommand
    {
        private readonly UserService userService;
        private readonly TeamService teamService;

        public InviteToTeamCommand(UserService userService, TeamService teamService)
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

            var teamName = args[0];
            var username = args[1];

            var isUserExist = userService.IsUserExistingByUsername(username);
            var isTeamExist = teamService.IsTeamExisting(teamName);
            if (!isTeamExist || !isUserExist)
            {
                throw new ArgumentException(Constants.ErrorMessages.TeamOrUserNotExist);
            }

            var currentUser = Session.CurrentUser;

            var isCreatorOfTeam = userService.IsUserCreatorOfTeam(currentUser.Id, teamName);
            var isCurrentUserMemberOfTeam = teamService.IsMemberOfTeam(teamName, currentUser.Username);
            var isUserToInviteMemberOfTeam = teamService.IsMemberOfTeam(teamName, username);

            if (!isCreatorOfTeam || isCurrentUserMemberOfTeam || isUserToInviteMemberOfTeam)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            var isUserAlreadyInvite = userService.IsUserInvitedFromTeam(username, teamName);
            if (isUserAlreadyInvite)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.InviteIsAlreadySent);
            }

            var result = string.Empty;

            var isInvitedUserCreatorOfTeam = userService.IsUserCreatorOfTeam(username, teamName);
            if (isInvitedUserCreatorOfTeam)
            {
                teamService.AddUserToTeam(teamName, username);
                result = string.Format(Constants.Succes.UserAddedToTeam, username, teamName);
            }
            else
            {
                userService.InviteToTeam(teamName, username);
                result = string.Format(Constants.Succes.InvitedSuccessfully, teamName, username);
            }

            return result;
        }
    }
}