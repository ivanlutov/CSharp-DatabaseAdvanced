namespace TeamBuilder.App.Core.Commands
{
    using System;
    using System.Text;
    using TeamBuilder.App.Utilities;
    using TeamBuilder.Service;
    public class ShowEventCommand : ICommand
    {
        private readonly EventService eventService;

        public ShowEventCommand(EventService eventService)
        {
            this.eventService = eventService;
        }

        public string Execute(params string[] args)
        {
            Check.CheckLength(1, args);

            var eventName = args[0];
            if (!eventService.IsEventExisting(eventName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.EventNotFound, eventName));
            }

            var eventToPrint = eventService.GetEventByName(eventName);

            var sb = new StringBuilder();
            sb.AppendLine($"{eventToPrint.Name} {eventToPrint.StartDate} {eventToPrint.EndDate}");
            sb.AppendLine($"{eventToPrint.Description}");
            sb.AppendLine("Teams:");

            if (eventToPrint.ParticipatingEventTeams.Count > 0)
            {
                foreach (var pe in eventToPrint.ParticipatingEventTeams)
                {
                    sb.AppendLine($"-{pe.Team.Name}");
                }
            }
            else
            {
                sb.AppendLine("-No existing teams");
            }

            return sb.ToString().Trim();
        }
    }
}