namespace BusTicketSystem.Client.Interfaces
{
    public interface ICommandDispatcher
    {
        string DispatchCommand(string[] commandParameters);
    }
}