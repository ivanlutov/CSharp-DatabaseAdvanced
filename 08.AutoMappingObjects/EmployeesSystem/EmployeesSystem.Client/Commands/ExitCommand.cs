namespace EmployeesSystem.Client.Commands
{
    using System;

    public class ExitCommand : ICommand
    {
        public string Execute(params string[] args)
        {
            Environment.Exit(0);
            return string.Empty;
        }
    }
}