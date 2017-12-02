namespace EmployeesSystem.Client.Commands
{
    public interface ICommand
    {
        string Execute(params string[] args);
    }
}