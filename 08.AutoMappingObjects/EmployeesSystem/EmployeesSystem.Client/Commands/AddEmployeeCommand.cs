namespace EmployeesSystem.Client.Commands
{
    using System;
    using EmployeesSystem.DtoModels;
    using EmployeesSystem.Services;

    public class AddEmployeeCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public AddEmployeeCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }
        public string Execute(params string[] args)
        {
            var firstName = args[0];
            var lastName = args[1];
            var salary = decimal.Parse(args[2]);

            if (employeeService.IsExistByFirstAndLastName(firstName, lastName))
            {
                throw new ArgumentException($"Employee {firstName} {lastName} already exist in database!");
            }

            var employeeDto = new EmployeeDto
            {
                FirstName = firstName,
                LastName = lastName,
                Salary = salary
            };

            employeeService.AddEmployee(employeeDto);

            return $"Employee {firstName} {lastName} added successfully!";
        }

    }
}