namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using PhotoShare.Data;

    public class LoginCommand : ICommand
    {
        public string Execute(string[] data)
        {
            var username = data[0];
            var password = data[1];

            using (PhotoShareContext context = new PhotoShareContext())
            {
                var user = context
                    .Users
                    .SingleOrDefault(u => u.Username == username && u.Password == password);

                if (Session.User != null)
                {
                    throw new ArgumentException("You should logout first!");
                }

                if (user == null)
                {
                    throw new ArgumentException("Invalid username or password!");
                }

                Session.User = user;

                return $"User {username} successfully logged in!";
            }
        }
    }
}