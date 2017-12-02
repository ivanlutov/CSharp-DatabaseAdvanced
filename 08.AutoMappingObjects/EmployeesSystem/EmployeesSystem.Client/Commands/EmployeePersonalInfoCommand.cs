namespace EmployeesSystem.Client.Commands
{
    using System;
    using EmployeesSystem.Services;
    public class EmployeePersonalInfoCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public EmployeePersonalInfoCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] args)
        {
            var employeeId = int.Parse(args[0]);

            if (!employeeService.IsExistById(employeeId))
            {
                throw new ArgumentException($"Employee with {employeeId} id does not exist in database!");
            }

            var employee = employeeService.PersonalById(employeeId);

            var birthday = "[no birthday specified]";
            if (employee.Birthday != null)
            {
                birthday = employee.Birthday.Value.ToString("dd-MM-yyyy");
            }

            var address = employee.Address ?? "[no address specified]";

            var result = $"ID: {employee.Id} - {employee.FirstName} {employee.LastName} - ${employee.Salary:F2}" + Environment.NewLine
                         + $"Birthday: {birthday}" + Environment.NewLine
                         + $"Address: {address}";

            return result;
        }
    }
}