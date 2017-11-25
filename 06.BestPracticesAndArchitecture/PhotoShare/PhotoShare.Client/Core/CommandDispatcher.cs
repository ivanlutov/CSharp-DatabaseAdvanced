using System.Linq;
using System.Reflection;

namespace PhotoShare.Client.Core
{
    using System;

    public class CommandDispatcher
    {
        public string DispatchCommand(string[] commandParameters)
        {
            string commandName = commandParameters[0] + "Command";
            var args = commandParameters.Skip(1).ToArray();

            var executingCommand = Assembly.GetExecutingAssembly().GetTypes().SingleOrDefault(t => t.Name == commandName);

            if (executingCommand == null)
            {
                throw new InvalidOperationException($"Command {commandName} not valid!");
            }

            
        }
    }
}
