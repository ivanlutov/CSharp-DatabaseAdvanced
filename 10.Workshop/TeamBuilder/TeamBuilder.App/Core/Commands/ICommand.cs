namespace TeamBuilder.App.Core.Commands
{
    public interface ICommand
    {
        string Execute(params string[] args);
    }
}