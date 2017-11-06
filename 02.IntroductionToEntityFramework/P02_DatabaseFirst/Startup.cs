namespace P02_DatabaseFirst
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using P02_DatabaseFirst.Data;
    using P02_DatabaseFirst.Data.Models;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    public class Startup
    {
        public static void Main()
        {
            var context = new SoftUniContext();

            //03. GetEmployeesFullInformation(context);
            //04. GetEmployeesWithSalaryOver50000(context);
            //05. GetEmployeesFromResearchAndDevelopment(context);
            //06. AddingANewAddressAndUpdatingEmployee(context);
            //07. FindEmployeesInPeriod(context);
            //08. GetAddressesByTownName(context);
            //09. PrintEmployeeWithId147(context);
            //10. GetDepartmentsWithMoreThan5Employees(context);
            //11. GetLatest10Projects(context);
            //12. IncreaseSalaries(context);
            //13. PrintEmployeesByFirstNameStartingWithSA(context);
            //14. DeleteProjectById(context);
            //15. RemoveTownAndAdressesByTownName(context);
        }

        private static void RemoveTownAndAdressesByTownName(SoftUniContext context)
        {
            var townName = Console.ReadLine();
            var town = context
                .Towns
                .Include(t => t.Addresses)
                .SingleOrDefault(t => t.Name == townName);

            var adressCount = 0;
            if (town != null)
            {
                adressCount = town.Addresses.Count;

                context
                    .Employees
                    .Where(e => e.AddressId != null && town.Addresses.Any(a => a.AddressId == e.AddressId))
                    .ToList()
                    .ForEach(e => e.Address = null);

                context.SaveChanges();

                context
                    .Addresses
                    .RemoveRange(town.Addresses);

                context.Towns.Remove(town);

                context.SaveChanges();
            }

            Console.WriteLine($"{adressCount} address in {townName} was deleted");
        }

        private static void DeleteProjectById(SoftUniContext context)
        {
            Console.Write("ProjectId for delete: ");
            var projectId = int.Parse(Console.ReadLine());
            var projectForRemove = context.Projects.Find(projectId);

            var epForRemove = context
                .EmployeesProjects
                .Where(ep => ep.ProjectId == projectId).ToList();

            context.
                EmployeesProjects
                .RemoveRange(epForRemove);

            context.Projects.Remove(projectForRemove);
            context.SaveChanges();

            context
                .Projects
                .Take(10)
                .Select(p => p.Name)
                .ToList()
                .ForEach(Console.WriteLine);
        }

        private static void PrintEmployeesByFirstNameStartingWithSA(SoftUniContext context)
        {
            context
                .Employees
                .Where(e => e.FirstName.ToUpper().StartsWith("SA"))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList()
                .ForEach(e => Console.WriteLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:F2})"));
        }

        private static void IncreaseSalaries(SoftUniContext context)
        {
            context
                .Employees
                .Include(e => e.Department)
                .Where(e => e.Department.Name == "Engineering" || e.Department.Name == "Tool Design" ||
                            e.Department.Name == "Marketing" || e.Department.Name == "Information Services")
                .ToList()
                .ForEach(e => e.Salary += e.Salary * 0.12M);

            context.SaveChanges();

            context
                .Employees
                .Where(e => e.Department.Name == "Engineering" || e.Department.Name == "Tool Design" ||
                            e.Department.Name == "Marketing" || e.Department.Name == "Information Services")
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList()
                .ForEach(e => Console.WriteLine($"{e.FirstName} {e.LastName} (${e.Salary:F2})"));
        }

        private static void GetLatest10Projects(SoftUniContext context)
        {
            var projects = context
                .Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .OrderBy(p => p.Name);

            using (var writer = new StreamWriter("projects.txt"))
            {
                foreach (var p in projects)
                {
                    string format = "M/d/yyyy h:mm:ss tt";
                    var startDate = p.StartDate.ToString(format, CultureInfo.InvariantCulture);

                    writer.WriteLine($"{p.Name}");
                    writer.WriteLine($"{p.Description}");
                    writer.WriteLine($"{startDate}");
                }
            }
        }

        private static void GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context
                .Departments
                .Include(d => d.Employees)
                .Where(d => d.Employees.Count > 5)
                .ToList();

            using (var writer = new StreamWriter("employees.txt"))
            {
                foreach (var d in departments
                                            .OrderBy(d => d.Employees.Count)
                                            .ThenBy(d => d.Name))
                {
                    writer.WriteLine($"{d.Name} - {d.Manager.FirstName} {d.Manager.LastName}");
                    foreach (var e in d.Employees
                                                .OrderBy(e => e.FirstName)
                                                .ThenBy(e => e.LastName))
                    {
                        writer.WriteLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                    }

                    writer.WriteLine(new string('-', 10));
                }
            }
        }

        private static void PrintEmployeeWithId147(SoftUniContext context)
        {
            var employee = context
                .Employees
                .Include(e => e.EmployeesProjects)
                .ThenInclude(p => p.Project)
                .FirstOrDefault(e => e.EmployeeId == 147);

            if (employee != null)
            {
                Console.WriteLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                foreach (var ep in employee.EmployeesProjects.OrderBy(p => p.Project.Name))
                {
                    Console.WriteLine(ep.Project.Name);
                }
            }
        }

        private static void GetAddressesByTownName(SoftUniContext context)
        {
            var addresses = context
                .Addresses
                .Include(a => a.Town)
                .Include(a => a.Employees)
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town.Name)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .ToList();

            foreach (var a in addresses)
            {
                Console.WriteLine($"{a.AddressText}, {a.Town.Name} - {a.Employees.Count} employees");
            }
        }

        private static void FindEmployeesInPeriod(SoftUniContext context)
        {

            var employees = context
                .Employees
                .Include(e => e.EmployeesProjects)
                .ThenInclude(ep => ep.Project)
                .Where(e => e.EmployeesProjects.Any(p => p.Project.StartDate.Year >= 2001 && p.Project.StartDate.Year <= 2003))
                .Take(30)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Manager,
                    e.EmployeesProjects
                })
                .ToList();

            foreach (var e in employees)
            {

                Console.WriteLine($"{e.FirstName} {e.LastName} - Manager: {e.Manager.FirstName} {e.Manager.LastName}");
                foreach (var p in e.EmployeesProjects)
                {
                    string format = "M/d/yyyy h:mm:ss tt";

                    string startDate = p.Project.StartDate.ToString(format, CultureInfo.InvariantCulture);
                    string endDate = p.Project.EndDate.ToString();

                    if (string.IsNullOrWhiteSpace(endDate))
                    {
                        endDate = "not finished";
                    }
                    else
                    {
                        endDate = p.Project.EndDate.Value.ToString(format, CultureInfo.InvariantCulture);
                    }

                    Console.WriteLine($"--{p.Project.Name} - {startDate} - {endDate}");
                }
            }
        }

        private static void AddingANewAddressAndUpdatingEmployee(SoftUniContext context)
        {
            var address = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            var employeeToSetAddress = context
                .Employees
                .FirstOrDefault(e => e.LastName == "Nakov");
            if (employeeToSetAddress != null)
            {
                employeeToSetAddress.Address = address;
            }

            context.SaveChanges();

            var employeesAddress = context
                .Employees
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .Select(e => e.Address.AddressText);

            foreach (var adr in employeesAddress)
            {
                Console.WriteLine(adr);
            }
        }

        private static void GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context
                .Employees
                .Where(e => e.Department.Name == "Research and Development")
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    DepartmentName = e.Department.Name,
                    e.Salary
                })
                .ToList();

            foreach (var e in employees)
            {
                Console.WriteLine($"{e.FirstName} {e.LastName} from {e.DepartmentName} - ${e.Salary:F2}");
            }
        }

        private static void GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employeesNames = context
                .Employees
                .Where(e => e.Salary > 50000)
                .Select(e => e.FirstName)
                .OrderBy(e => e);

            foreach (var e in employeesNames)
            {
                Console.WriteLine(e);
            }
        }

        private static void GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context
                .Employees;

            foreach (var e in employees)
            {
                Console.WriteLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:F2}");
            }
        }
    }
}
