namespace TeamBuilder.App.Utilities
{
    public static class Constants
    {
        public const int MinUsernameLength = 3;
        public const int MaxUsernameLength = 25;

        public const int MaxFirstNameLength = 25;

        public const int MaxLastNameLength = 25;

        public const int MinPasswordLength = 6;
        public const int MaxPasswordLength = 30;

        public const int MaxEventNameLength = 25;
        public const int MaxEventDescriptionLength = 250;

        public const int MaxTeamNameLength = 25;
        public const int MaxTeamDescriptionLength = 32;
        public const int TeamAcronymLength = 3;

        public const string DateTimeFormat = "dd/MM/yyyy HH:mm";

        public static class ErrorMessages
        {
            // Common error messages.
            public const string InvalidArgumentsCount = "Invalid arguments count!";

            public const string LogoutFirst = "You should logout first!";
            public const string LoginFirst = "You should login first!";

            public const string TeamOrUserNotExist = "Team or user does not exist!";
            public const string InviteIsAlreadySent = "Invite is already sent!";

            public const string NotAllowed = "Not allowed!";
            public const string NotAllowedDisbandTeam = "Command not allowed. Use DisbandTeam instead.";

            public const string TeamNotFound = "Team {0} not found!";
            public const string UserNotFound = "User {0} not found!";
            public const string EventNotFound = "Event {0} not found!";
            public const string InviteNotFound = "Invite from {0} is not found!";
            public const string EventNameLengthInvalid = "Invalid event name length!";
            public const string EventDescriptionLengthInvalid = "Invalid event description length!";
            public const string NotMemberOfTeam = "User {0} is not a member in {1}!";

            public const string NotPartOfTeam = "User {0} is not a member in {1}!";

            public const string CommandNotAllowed = "Command not allowed. Use {0} instead.";
            public const string CannotAddSameTeamTwice = "Cannot add same team twice!";

            // User error messages.
            public const string UsernameNotValid = "Username {0} not valid!";
            public const string PasswordNotValid = "Password {0} not valid!";
            public const string PasswordDoesNotMatch = "Passwords do not match!";
            public const string AgeNotValid = "Age not valid!";
            public const string GenderNotValid = "Gender should be either “Male” or “Female”!";
            public const string UsernameIsTaken = "Username {0} is already taken!";
            public const string UserOrPasswordIsInvalid = "Invalid username or password!";
            public const string TeamNameLengthNotValid = "Invalid team name length!";

            public const string InvalidDateFormat =
                                      "Please insert the dates in format: [dd/MM/yyyy HH:mm]!";

            public const string InvalidOrderDate = "Start date should be before end date.";

            // Team error messages.
            public const string InvalidAcronym = "Acronym {0} not valid!";
            public const string TeamExists = "Team {0} exists!";
        }

        public static class Succes
        {
            public const string UserRegisterSuccesfully = "User {0} was registered successfully!";
            public const string LoginSuccessfully = "User {0} successfully logged in!";
            public const string LogoutSuccessfully = "User {0} successfully logged out!";
            public const string DeleteUserSuccessfully = "User {0} was deleted successfully!";
            public const string EventCreatedSuccessfully = "Event {0} was created successfully!";
            public const string TeamCreatedSuccessfully = "Team {0} successfully created!";
            public const string InvitedSuccessfully = "Team {0} invited {1}!";
            public const string UserAddedToTeam = "User {0} joined team {1}!";
            public const string DeclineInvitation = "Invite from {0} declined.";
            public const string KickedMember = "User {0} was kicked from {1}!";
            public const string DisbandTeam = "{0} has disbanded!";
            public const string AddTeamToEvent = "Team {0} added for {1}!";
        }
    }
}