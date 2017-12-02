namespace EmployeesSystem.Client
{
    using AutoMapper;
    using System;
    using Core;
    using Data;
    using Data.Configurations;
    using Services;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public class Application
    {
        public static void Main()
        {
            var serviceProvider = ConfigureService();
            var engine = new Engine(serviceProvider);
            engine.Run();
        }

        static IServiceProvider ConfigureService()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<EmployeeSystemContext>(opt =>
                opt.UseSqlServer(Configuration.ConnectionString));

            serviceCollection.AddAutoMapper(cfg => cfg.AddProfile<EmployeesSystemProfile>());

            serviceCollection.AddTransient<EmployeeService>();

            var serviceProvider = serviceCollection.BuildServiceProvider();
            return serviceProvider;
        }
    }
}
