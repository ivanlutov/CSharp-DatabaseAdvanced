namespace EmployeesSystem.Client.Commands
{
    using System.Text;
    using EmployeesSystem.Services;
    using System.Linq;

    public class ListEmployeesOlderThanCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public ListEmployeesOlderThanCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }
        public string Execute(params string[] args)
        {
            var age = int.Parse(args[0]);

            var employees = employeeService
                .GetEmployeesWithManagersByAge(age)
                .OrderByDescending(e => e.Salary);

            var sb = new StringBuilder();

            foreach (var e in employees)
            {
                var managerName = "[no manager]";
                if (e.ManagerName != null)
                {
                    managerName = e.ManagerName;
                }

                sb.AppendLine($"{e.FirstName} {e.LastName} - ${e.Salary:F2} - Manager: {managerName}");
            }

            return sb.ToString().Trim();
        }
    }
}