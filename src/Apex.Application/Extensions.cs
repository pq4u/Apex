using Apex.Application.Abstractions;
using Apex.Application.Services;
using Apex.Domain.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Apex.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var applicationAssembly = Assembly.GetExecutingAssembly();

        services.Scan(s => s.FromAssemblies(applicationAssembly)
            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(s => s.FromAssemblies(applicationAssembly)
            .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddApplicationServices(configuration);

        return services;
    }

    private static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TelemetryIngestionOptions>(options =>
        {
            configuration.GetSection(TelemetryIngestionOptions.SectionName).Bind(options);
        });

        services.AddScoped<ITelemetryIngestionService, TelemetryIngestionService>();
        services.AddScoped<ISessionManagementService, SessionManagementService>();
        services.AddScoped<IDriverAssociationService, DriverAssociationService>();
        services.AddScoped<ILapService, LapService>();
        services.AddScoped<IMeetingIngestionService, MeetingIngestionService>();

        return services;
    }
}