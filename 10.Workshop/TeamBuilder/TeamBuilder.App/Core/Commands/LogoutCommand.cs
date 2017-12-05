namespace TeamBuilder.App.Core.Commands
{
    using System;
    using TeamBuilder.App.Utilities;

    public class LogoutCommand : ICommand
    {
        public string Execute(params string[] args)
        {
            Check.CheckLength(0, args);

            if (Session.CurrentUser == null)
            {
                throw new ArgumentException(Constants.ErrorMessages.LoginFirst);
            }

            var username = Session.CurrentUser.Username;
            Session.CurrentUser = null;

            return string.Format(Constants.Succes.LogoutSuccessfully, username);
        }
    }
}