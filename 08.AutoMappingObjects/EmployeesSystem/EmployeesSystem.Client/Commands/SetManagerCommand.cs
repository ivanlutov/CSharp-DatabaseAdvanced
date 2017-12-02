namespace EmployeesSystem.Client.Commands
{
    using System;
    using EmployeesSystem.Services;

    public class SetManagerCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public SetManagerCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }
        public string Execute(params string[] args)
        {
            var employeeId = int.Parse(args[0]);
            var managerId = int.Parse(args[1]);

            if (!employeeService.IsExistById(employeeId))
            {
                throw new ArgumentException($"Employee with {employeeId} id does not exist in database!");
            }

            if (!employeeService.IsExistById(managerId))
            {
                throw new ArgumentException($"Manager with {employeeId} id does not exist in database!");
            }

            var names = employeeService.SetManagerByEmployeeIdAndManagerId(employeeId, managerId);

            var employeeName = names[0];
            var managerName = names[1];

            return $"Successfully set {managerName} for manager of {employeeName}";
        }
    }
}