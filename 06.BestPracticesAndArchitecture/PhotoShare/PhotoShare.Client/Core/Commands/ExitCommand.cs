namespace PhotoShare.Client.Core.Commands
{
    using System;

    public class ExitCommand : ICommand
    {
        public string Execute(string[] data)
        {
            Console.WriteLine("Good Bye!");
            Environment.Exit(0);

            return string.Empty;
        }
    }
}
