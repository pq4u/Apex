using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using Serilog;

namespace Apex.Infrastructure.DAL;

public static class MigrationExtensions
{
    public static async Task<IApplicationBuilder> MigrateDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        await MigrateDatabaseInternal(scope.ServiceProvider);
        return app;
    }

    public static async Task<IHost> MigrateDatabase(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        await MigrateDatabaseInternal(scope.ServiceProvider);
        return host;
    }

    private static async Task MigrateDatabaseInternal(IServiceProvider services)
    {
        try
        {
            var dbContext = services.GetRequiredService<ApexDbContext>();
            var configuration = services.GetRequiredService<IConfiguration>();

            Log.Information("Starting database migration");

            await dbContext.Database.MigrateAsync();
            Log.Information("EF Core migrations applied successfully");

            await SetupTimescaleDB(configuration);

            Log.Information("Database migration completed successfully");
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<ApexDbContext>>();
            logger.LogError(ex, "An error occurred while migrating the database");
            throw;
        }
    }

    private static async Task SetupTimescaleDB(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ApexDb");

        using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        var timescaleScript = @"
                CREATE EXTENSION IF NOT EXISTS timescaledb CASCADE;

                CREATE SCHEMA IF NOT EXISTS telemetry;

                CREATE TABLE IF NOT EXISTS telemetry.car_data (
                    time TIMESTAMPTZ NOT NULL,
                    session_id INT NOT NULL,
                    driver_id INT NOT NULL,
                    speed SMALLINT,
                    rpm SMALLINT,
                    gear SMALLINT,
                    throttle SMALLINT,
                    brake SMALLINT,
                    drs SMALLINT,
                    n_gear SMALLINT
                );

                SELECT create_hypertable(
                    'telemetry.car_data', 
                    'time',
                    chunk_time_interval => INTERVAL '1 day',
                    if_not_exists => TRUE
                );

                CREATE INDEX IF NOT EXISTS idx_car_data_session_driver 
                    ON telemetry.car_data (session_id, driver_id, time DESC);";

        using var command = new NpgsqlCommand(timescaleScript, connection);
        await command.ExecuteNonQueryAsync();

        Log.Information("TimescaleDB setup completed");
    }
}