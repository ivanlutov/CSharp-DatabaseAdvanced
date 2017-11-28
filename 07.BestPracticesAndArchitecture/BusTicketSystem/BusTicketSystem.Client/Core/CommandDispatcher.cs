namespace BusTicketSystem.Client.Core
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using BusTicketSystem.Client.Interfaces;
    public class CommandDispatcher : ICommandDispatcher
    {
        private const string SuffixCommand = "Command";
        public string DispatchCommand(string[] commandParameters)
        {
            var commandArgs = commandParameters[0].Split('-');
            var commandName = ToTitleCase(commandArgs[0]) + ToTitleCase(commandArgs[1]) + SuffixCommand;
            var args = commandParameters.Skip(1).ToArray();

            //var executingCommand = Assembly.GetExecutingAssembly()
            //    .GetTypes()
            //    .SingleOrDefault(t => t.Name == commandName && t.GetInterfaces().Any(i => i == typeof(ICommand)));

            var executingCommand = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .SingleOrDefault(t => t.Name == commandName
                                      && t.GetInterfaces().Any(i => i == typeof(ICommand)));

            if (executingCommand == null)
            {
                throw new InvalidOperationException("Invalid command!");
            }

            var instance = (ICommand)Activator.CreateInstance(executingCommand);
            var result = instance.Execute(args);
            return result;
        }

        private string ToTitleCase(string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }
    }
}