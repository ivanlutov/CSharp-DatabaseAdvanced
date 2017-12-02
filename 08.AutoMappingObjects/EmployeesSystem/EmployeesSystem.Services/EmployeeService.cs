namespace EmployeesSystem.Services
{
    using System;
    using Data;
    using System.Linq;
    using AutoMapper;
    using EmployeesSystem.DtoModels;
    using EmployeesSystem.Models;
    using AutoMapper.QueryableExtensions;

    public class EmployeeService
    {
        private readonly EmployeeSystemContext context;

        public EmployeeService(EmployeeSystemContext context)
        {
            this.context = context;
        }

        public EmployeeDto ById(int id)
        {
            var employee = context.Employees.Find(id);

            var employeeDto = Mapper.Map<EmployeeDto>(employee);

            return employeeDto;
        }

        public ManagerDto GetManagerDtoById(int id)
        {
            var employee = context.Employees.Find(id);

            var managerDto = Mapper.Map<ManagerDto>(employee);

            return managerDto;
        }

        public EmployeeWithManagerDto[] GetEmployeesWithManagersByAge(int age)
        {
            var employees = context
                .Employees
                .Where(e => (DateTime.Now.Year - e.Birthday.Value.Year) > age)
                .ProjectTo<EmployeeWithManagerDto>()
                .ToArray();

            return employees;
        }

        public void AddEmployee(EmployeeDto dto)
        {
            var employee = Mapper.Map<Employee>(dto);

            context.Employees.Add(employee);

            context.SaveChanges();
        }

        public string SetBirthday(int id, DateTime date)
        {
            var employee = context.Employees.Find(id);

            employee.Birthday = date;

            context.SaveChanges();

            return $"{employee.FirstName} {employee.LastName}";
        }

        public string SetAddress(int id, string address)
        {
            var employee = context.Employees.Find(id);

            employee.Address = address;

            context.SaveChanges();

            return $"{employee.FirstName} {employee.LastName}";
        }

        public EmployeePersonalDto PersonalById(int id)
        {
            var employee = context.Employees.Find(id);

            var employeeDto = Mapper.Map<EmployeePersonalDto>(employee);

            return employeeDto;
        }

        public string[] SetManagerByEmployeeIdAndManagerId(int employeeId, int managerId)
        {
            var employee = context.Employees.Find(employeeId);
            var manager = context.Employees.Find(managerId);

            employee.Manager = manager;
            manager.Subordinates.Add(employee);
            context.SaveChanges();

            var names = new string[]
            {
                $"{employee.FirstName} {employee.LastName}",
                $"{manager.FirstName} {manager.LastName}"
            };

            return names;
        }

        public bool IsExistByFirstAndLastName(string firstName, string lastName)
        {
            return context.Employees.Any(e => e.FirstName == firstName && e.LastName == lastName);
        }

        public bool IsExistById(int id)
        {
            return context.Employees.Any(e => e.Id == id);
        }
    }
}
