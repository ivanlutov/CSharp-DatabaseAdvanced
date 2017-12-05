namespace TeamBuilder.App.Core.Commands
{
    using System;
    using TeamBuilder.App.Utilities;
    using TeamBuilder.Service;
    public class LoginCommand : ICommand
    {
        private readonly UserService userService;

        public LoginCommand(UserService userService)
        {
            this.userService = userService;
        }

        public string Execute(params string[] args)
        {
            Check.CheckLength(2, args);

            var username = args[0];
            var password = args[1];

            if (!userService.IsUserExistingByUsernameAndPassword(username, password))
            {
                throw new ArgumentException(Constants.ErrorMessages.UserOrPasswordIsInvalid);
            }

            if (Session.CurrentUser != null)
            {
                throw new ArgumentException(Constants.ErrorMessages.LogoutFirst);
            }

            var user = userService.GetUserByUsernameAndPassword(username, password);

            Session.CurrentUser = user;

            return string.Format(Constants.Succes.LoginSuccessfully, username);
        }
    }
}