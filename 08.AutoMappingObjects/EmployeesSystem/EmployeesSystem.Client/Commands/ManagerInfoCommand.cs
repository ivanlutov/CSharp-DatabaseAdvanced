namespace EmployeesSystem.Client.Commands
{
    using System;
    using System.Text;
    using EmployeesSystem.Services;
    public class ManagerInfoCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public ManagerInfoCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] args)
        {
            var managerId = int.Parse(args[0]);

            if (!employeeService.IsExistById(managerId))
            {
                throw new ArgumentException($"Manager with {managerId} id does not exist in database!");
            }

            var manager = employeeService.GetManagerDtoById(managerId);

            var sb = new StringBuilder();

            sb.AppendLine($"{manager.FirstName} {manager.LastName} | Employees: {manager.SubordinatesCount}");
            foreach (var subordinate in manager.Subordinates)
            {
                sb.AppendLine($"  - {subordinate.FirstName} {subordinate.LastName} - ${subordinate.Salary:F2}");
            }

            return sb.ToString().Trim();
        }
    }
}