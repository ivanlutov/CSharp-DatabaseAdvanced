namespace EmployeesSystem.DtoModels
{
    using System.Collections.Generic;

    public class ManagerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int SubordinatesCount { get; set; }

        public List<EmployeeDto> Subordinates { get; set; } = new List<EmployeeDto>();
    }
}