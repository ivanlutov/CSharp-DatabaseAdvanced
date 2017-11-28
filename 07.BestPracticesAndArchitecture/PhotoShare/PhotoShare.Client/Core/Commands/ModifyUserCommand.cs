namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using PhotoShare.Data;

    public class ModifyUserCommand : ICommand
    {
        // ModifyUser <username> <property> <new value>
        // For example:
        // ModifyUser <username> Password <NewPassword>
        // ModifyUser <username> BornTown <newBornTownName>
        // ModifyUser <username> CurrentTown <newCurrentTownName>
        // !!! Cannot change username
        public string Execute(string[] data)
        {
            var username = data[0];
            var property = data[1];
            var newValue = data[2];

            using (PhotoShareContext context = new PhotoShareContext())
            {
                var user = context.Users.SingleOrDefault(u => u.Username == username);
                if (user == null)
                {
                    throw new ArgumentException($"User {username} not found!");
                }

                if (Session.User == null || Session.User.Username != user.Username)
                {
                    throw new InvalidOperationException("Invalid credentials!");
                }

                switch (property)
                {
                    case "Password":
                        user.Password = newValue;
                        break;
                    case "BornTown":
                        var bornTown = context.Towns.SingleOrDefault(t => t.Name == newValue);
                        if (bornTown == null)
                        {
                            throw new ArgumentException(
                                $"Value {newValue} not valid!"
                                + Environment.NewLine +
                                $"Town {newValue} not found!");
                        }

                        user.BornTown = bornTown;
                        break;
                    case "CurrentTown":
                        var currentTown = context.Towns.SingleOrDefault(t => t.Name == newValue);
                        if (currentTown == null)
                        {
                            throw new ArgumentException(
                                $"Value {newValue} not valid!"
                                + Environment.NewLine +
                                $"Town {newValue} not found!");
                        }

                        user.CurrentTown = currentTown;
                        break;
                    default: throw new ArgumentException($"Property {property} not supported!");
                }

                context.SaveChanges();
            }

            return $"User {username} {property} is {newValue}.";
        }
    }
}
