namespace EmployeesSystem.Client.Commands
{
    using System;
    using EmployeesSystem.Services;
    public class SetBirthdayCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public SetBirthdayCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] args)
        {
            var employeeId = int.Parse(args[0]);
            var dateToString = args[1];
            DateTime date = DateTime.ParseExact(dateToString, "dd-MM-yyyy", null);

            if (!employeeService.IsExistById(employeeId))
            {
                throw new ArgumentException($"Employee with {employeeId} id does not exist in database!");
            }

            var employeeName = employeeService.SetBirthday(employeeId, date);

            return $"{employeeName}'s birthday was set to {dateToString}";
        }
    }
}