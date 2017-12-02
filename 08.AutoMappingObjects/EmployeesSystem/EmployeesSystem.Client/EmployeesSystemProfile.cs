namespace EmployeesSystem.Client
{
    using EmployeesSystem.DtoModels;
    using EmployeesSystem.Models;
    using AutoMapper;
    public class EmployeesSystemProfile : Profile
    {
        public EmployeesSystemProfile()
        {
            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeDto, Employee>();
            CreateMap<Employee, EmployeePersonalDto>();
            CreateMap<Employee, ManagerDto>()
                .ForMember(dest => dest.SubordinatesCount,
                           opt => opt.MapFrom(src => src.Subordinates.Count))
                .ForMember(dest => dest.Subordinates,
                           opt => opt.MapFrom(src => src.Subordinates));
            CreateMap<Employee, EmployeeWithManagerDto>();
        }
    }
}
