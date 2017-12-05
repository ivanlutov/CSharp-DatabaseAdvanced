namespace TeamBuilder.App.Core.Commands
{
    using System;
    using TeamBuilder.App.Utilities;
    using TeamBuilder.Models;
    using TeamBuilder.Service;

    public class DeleteUserCommand : ICommand
    {
        private readonly UserService userService;

        public DeleteUserCommand(UserService userService)
        {
            this.userService = userService;
        }

        public string Execute(params string[] args)
        {
            Check.CheckLength(0, args);

            if (Session.CurrentUser == null)
            {
                throw new ArgumentException(Constants.ErrorMessages.LoginFirst);
            }

            var user = Session.CurrentUser;
            userService.Delete(user);
            Session.CurrentUser = null;

            return string.Format(Constants.Succes.DeleteUserSuccessfully, user.Username);
        }
    }
}