namespace PhotoShare.Client.Core.Commands
{
    using System;

    public class LogoutCommand : ICommand
    {
        public string Execute(string[] data)
        {
            if (Session.User == null)
            {
                throw new ArgumentException("You should log in first in order to logout.");
            }

            var username = Session.User.Username;
            Session.User = null;

            return $"User {username} successfully logged out!";
        }
    }
}