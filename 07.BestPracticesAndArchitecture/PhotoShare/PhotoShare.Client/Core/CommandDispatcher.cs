namespace PhotoShare.Client.Core
{
    using System;
    using System.Linq;
    using System.Reflection;
    using PhotoShare.Client.Core.Commands;

    public class CommandDispatcher
    {
        public string DispatchCommand(string[] commandParameters)
        {
            string commandName = commandParameters[0] + "Command";
            var args = commandParameters.Skip(1).ToArray();

            var executingCommand = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .SingleOrDefault(t => t.Name == commandName
                 && t.GetInterfaces().Any(i => i == typeof(ICommand)));

            if (executingCommand == null)
            {
                throw new InvalidOperationException($"Command {commandName} not valid!");
            }

            var command = (ICommand)Activator.CreateInstance(executingCommand);
            string result = command.Execute(args);
            return result;
        }
    }
}
