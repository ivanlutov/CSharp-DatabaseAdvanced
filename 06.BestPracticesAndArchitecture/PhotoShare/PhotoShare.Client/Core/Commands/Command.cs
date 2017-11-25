namespace PhotoShare.Client.Core.Commands
{
    public abstract class Command
    {
        public string Execute(string[] data);
    }
}