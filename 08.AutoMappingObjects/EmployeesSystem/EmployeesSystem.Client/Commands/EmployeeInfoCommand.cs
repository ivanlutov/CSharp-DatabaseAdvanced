namespace EmployeesSystem.Client.Commands
{
    using System;
    using EmployeesSystem.Services;
    public class EmployeeInfoCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public EmployeeInfoCommand(EmployeeService employeeService)
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

            var employee = employeeService.ById(employeeId);

            return $"ID: {employee.Id} - {employee.FirstName} {employee.LastName} - ${employee.Salary:F2}";
        }
    }
}