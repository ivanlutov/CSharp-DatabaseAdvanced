namespace PhotoShare.Client.Core.Commands
{
    using System;
    using Models;
    using Data;
    using System.Linq;

    public class RegisterUserCommand : ICommand
    {
        // RegisterUser <username> <password> <repeat-password> <email>
        public string Execute(string[] data)
        {
            string username = data[0];
            string password = data[1];
            string repeatPassword = data[2];
            string email = data[3];

            using (PhotoShareContext context = new PhotoShareContext())
            {
                if (context.Users.SingleOrDefault(u => u.Username == username) != null)
                {
                    throw new InvalidOperationException($"Username {username} is already taken!");
                }

                if (password != repeatPassword)
                {
                    throw new ArgumentException("Passwords do not match!");
                }
            }

            User user = new User
            {
                Username = username,
                Password = password,
                Email = email,
                IsDeleted = false,
                RegisteredOn = DateTime.Now,
                LastTimeLoggedIn = DateTime.Now
            };

            using (PhotoShareContext context = new PhotoShareContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }

            return "User " + user.Username + " was registered successfully!";
        }
    }
}
