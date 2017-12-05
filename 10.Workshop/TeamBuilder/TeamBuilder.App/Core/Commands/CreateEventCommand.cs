namespace TeamBuilder.App.Core.Commands
{
    using System;
    using TeamBuilder.App.Utilities;
    using System.Globalization;
    using TeamBuilder.Service;


    public class CreateEventCommand : ICommand
    {
        private readonly EventService eventService;

        public CreateEventCommand(EventService eventService)
        {
            this.eventService = eventService;
        }

        public string Execute(params string[] args)
        {
            Check.CheckLength(6, args);

            if (Session.CurrentUser == null)
            {
                throw new ArgumentException(Constants.ErrorMessages.LoginFirst);
            }

            var name = args[0];

            if (name.Length > Constants.MaxEventNameLength)
            {
                throw new ArgumentException(Constants.ErrorMessages.EventNameLengthInvalid);
            }

            var description = args[1];

            if (description.Length > Constants.MaxEventDescriptionLength)
            {
                throw new ArgumentException(Constants.ErrorMessages.EventDescriptionLengthInvalid);
            }

            var startDateStr = args[2] + " " + args[3];
            DateTime startDate;
            var isParseStartDate = DateTime.TryParseExact(startDateStr, Constants.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);

            var endDateStr = args[4] + " " + args[5];
            DateTime endDate;
            var isParseEndDate = DateTime.TryParseExact(endDateStr, Constants.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);

            if (!isParseStartDate || !isParseEndDate)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.InvalidDateFormat);
            }

            if (startDate > endDate)
            {
                throw new ArgumentException(Constants.ErrorMessages.InvalidOrderDate);
            }

            var creatorId = Session.CurrentUser.Id;

            eventService.Create(name, description, startDate, endDate, creatorId);

            return string.Format(Constants.Succes.EventCreatedSuccessfully, name);
        }
    }
}