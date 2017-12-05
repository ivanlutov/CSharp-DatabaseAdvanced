namespace TeamBuilder.App
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using TeamBuilder.Data;
    using TeamBuilder.Service;
    using AutoMapper;
    using TeamBuilder.App.Core;

    public class Application
    {
        public static void Main(string[] args)
        {
            var context = new TeamBuilderContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var serviceProvider = ConfigureService();
            var engine = new Engine(serviceProvider);
            engine.Run();
        }
        static IServiceProvider ConfigureService()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<TeamBuilderContext>(opt =>
                opt.UseSqlServer(ConnectionCofiguration.ConnectionString));

            serviceCollection.AddAutoMapper(cfg => cfg.AddProfile<TeamBuilderProfile>());

            serviceCollection.AddTransient<EventService>();
            serviceCollection.AddTransient<InvitationService>();
            serviceCollection.AddTransient<TeamService>();
            serviceCollection.AddTransient<UserService>();
            
            var serviceProvider = serviceCollection.BuildServiceProvider();
            return serviceProvider;
        }
    }
}
