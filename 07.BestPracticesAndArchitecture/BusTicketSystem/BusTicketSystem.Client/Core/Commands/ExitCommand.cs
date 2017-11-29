namespace BusTicketSystem.Client.Core.Commands
{
    using System;
    using BusTicketSystem.Client.Interfaces;

    public class ExitCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Environment.Exit(0);
            return string.Empty;
        }
    }
}