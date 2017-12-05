namespace TeamBuilder.App.Core.Commands
{
    using System;
    using TeamBuilder.App.Utilities;

    public class ExitCommand : ICommand
    {
        public string Execute(params string[] args)
        {
            Check.CheckLength(0, args);

            Environment.Exit(0);
            return string.Empty;
        }
    }
}