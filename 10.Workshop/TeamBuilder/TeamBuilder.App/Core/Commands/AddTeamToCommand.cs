using System;
using TeamBuilder.App.Utilities;
using TeamBuilder.Service;

namespace TeamBuilder.App.Core.Commands
{
    public class AddTeamToCommand : ICommand
    {
        private readonly TeamService teamService;
        private readonly EventService eventService;
        private readonly UserService userService;

        public AddTeamToCommand(TeamService teamService, EventService eventService, UserService userService)
        {
            this.teamService = teamService;
            this.eventService = eventService;
            this.userService = userService;
        }

        public string Execute(params string[] args)
        {
            Check.CheckLength(2, args);

            if (Session.CurrentUser == null)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LoginFirst);
            }

            var eventName = args[0];
            if (!eventService.IsEventExisting(eventName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.EventNotFound, eventName));
            }

            var teamName = args[1];
            if (!teamService.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            var loggedUserId = Session.CurrentUser.Id;
            if (!userService.IsUserCreatorOfEvent(loggedUserId, eventName))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            if (teamService.IsAddedToEvent(teamName, eventName))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.CannotAddSameTeamTwice);
            }

            teamService.AddTeamToEvent(teamName, eventName);

            return string.Format(Constants.Succes.AddTeamToEvent, teamName, eventName);
        }
    }
}