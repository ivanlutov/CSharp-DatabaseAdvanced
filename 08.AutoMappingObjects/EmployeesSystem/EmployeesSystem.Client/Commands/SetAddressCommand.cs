namespace EmployeesSystem.Client.Commands
{
    using System;
    using EmployeesSystem.Services;
    public class SetAddressCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public SetAddressCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] args)
        {
            var employeeId = int.Parse(args[0]);
            var address = args[1];

            if (!employeeService.IsExistById(employeeId))
            {
                throw new ArgumentException($"Employee with {employeeId} id does not exist in database!");
            }

            var employeeName = employeeService.SetAddress(employeeId, address);

            return $"{employeeName}'s address was set to {address}";
        }
    }
}