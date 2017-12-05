
namespace TeamBuilder.App.Core
{
    using System;
    using System.Linq;
    using System.Reflection;
    using TeamBuilder.App.Core.Commands;

    public class CommandDispatcher
    {
        private const string Suffix = "Command";

        public static ICommand Dispatch(IServiceProvider provider, string commandName)
        {
            commandName = commandName + Suffix;

            var commandType = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .SingleOrDefault(t => t.Name == commandName && t.GetInterfaces().Any(i => i == typeof(ICommand)));

            if (commandType == null)
            {
                throw new ArgumentException("Invalid command!");
            }

            var constructor = commandType.GetConstructors().First();

            var constructorParams = constructor
                .GetParameters()
                .Select(pi => pi.ParameterType)
                .ToArray();

            var constructorArgs = constructorParams
                .Select(provider.GetService)
                .ToArray();

            var command = (ICommand)constructor.Invoke(constructorArgs);

            return command;
        }
    }
}