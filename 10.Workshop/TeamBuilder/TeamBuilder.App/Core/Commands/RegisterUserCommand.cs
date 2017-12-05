namespace TeamBuilder.App.Core.Commands
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using TeamBuilder.App.Utilities;
    using TeamBuilder.Models;
    using TeamBuilder.Service;

    public class RegisterUserCommand : ICommand
    {
        private readonly UserService userService;

        public RegisterUserCommand(UserService userService)
        {
            this.userService = userService;
        }
        public string Execute(params string[] args)
        {
            Check.CheckLength(7, args);

            var username = args[0];

            if (username.Length < Constants.MinUsernameLength || username.Length > Constants.MaxUsernameLength)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.UsernameNotValid, username));
            }

            var password = args[1];

            if (!password.Any(char.IsDigit) || !password.Any(char.IsUpper))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.PasswordNotValid, password));
            }

            var repeatedPassword = args[2];

            if (password != repeatedPassword)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.PasswordDoesNotMatch);
            }

            var firstName = args[3];
            var lastName = args[4];

            int age;
            bool isNumber = int.TryParse(args[5], out age);

            if (!isNumber || age <= 0)
            {
                throw new ArgumentException(Constants.ErrorMessages.AgeNotValid);
            }

            Gender gender;
            bool isGenderValid = Enum.TryParse(args[6], out gender);

            if (!isGenderValid)
            {
                throw new ArgumentException(Constants.ErrorMessages.GenderNotValid);
            }

            if (userService.IsUserExistingByUsername(username))
            {
                throw new InvalidOperationException(string.Format(Constants.ErrorMessages.UsernameIsTaken, username));
            }

            userService.Register(username, password, firstName, lastName, age, gender);

            return string.Format(Constants.Succes.UserRegisterSuccesfully, username);

        }
    }
}