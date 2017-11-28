namespace BusTicketSystem.Client.Interfaces
{
    public interface ICommand
    {
        string Execute(string[] args);
    }
}