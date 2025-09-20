using Apex.Domain.Repositories;
using Apex.Infrastructure.DAL;
using Apex.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data;
using Apex.Application.Abstractions;
using Apex.Infrastructure.DAL.Decorators;

namespace Apex.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDatabase(configuration)
            .AddRepositories();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITelemetryRepository, TelemetryRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<ILapRepository, LapRepository>();
        services.AddScoped<ISessionDriverRepository, SessionDriverRepository>();
        services.AddScoped<IMeetingRepository, MeetingRepository>();
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<IDriverRepository, DriverRepository>();
        
        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ApexDb");

        services.AddDbContext<ApexDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly(typeof(ApexDbContext).Assembly.FullName);
                npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "domain");
                npgsqlOptions.CommandTimeout(120);
                //npgsqlOptions.EnableRetryOnFailure(3);
            });

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (environment == "Development")
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
                options.LogTo(Console.WriteLine);
            }
        });
        
        services.AddScoped<IDbConnection>(sp =>
        {
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            return connection;
        });

        services.AddScoped<IUnitOfWork, PostgresUnitOfWork>();
        services.TryDecorate(typeof(ICommandHandler<>), typeof(UnitOfWorkCommandHandlerDecorator<>));

        return services;
    }
}
