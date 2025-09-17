using Apex.Application.Abstractions;
using Apex.Application.Commands.Drivers;
using Apex.Application.Commands.Laps;
using Apex.Application.Commands.Meetings;
using Apex.Application.Commands.Sessions;
using Apex.Application.Commands.Telemetry;
using Apex.Application.Queries.Drivers;
using Apex.Application.Queries.Sessions;
using Apex.Domain.Entities;
using Apex.Domain.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Apex.Worker;

public class DataIngestionWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public DataIngestionWorker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Log.Information("Data Ingestion Worker started");

        var sessionKey = 7783;

        using (var scope = _serviceProvider.CreateScope())
        {
            try
            {
                await IngestMeetingsAsync(scope.ServiceProvider, stoppingToken);
                await IngestSessionDataAsync(scope.ServiceProvider, sessionKey, stoppingToken);
                Log.Information("Successfully ingested session {SessionKey}", sessionKey);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to ingest session {SessionKey}", sessionKey);
            }
        }
    }

    private async Task IngestMeetingsAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        Log.Information("Starting ingestion meeting list from OpenF1 API");
        
        var ingestMeetingsHandler = serviceProvider.GetRequiredService<ICommandHandler<IngestMeetingsCommand>>();
        await ingestMeetingsHandler.HandleAsync(new IngestMeetingsCommand(), cancellationToken);

    }
    
    // private async Task IngestSessionsAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
    // {
    //     Log.Information("Starting ingestion session list from OpenF1 API");
    // }

    private async Task IngestSessionDataAsync(IServiceProvider serviceProvider, int sessionKey, CancellationToken cancellationToken)
    {
        Log.Information("Starting ingestion for session {SessionKey}", sessionKey);

        try
        {
            var createSessionHandler = serviceProvider.GetRequiredService<ICommandHandler<CreateSessionCommand>>();
            await createSessionHandler.HandleAsync(new CreateSessionCommand(sessionKey), cancellationToken);

            var getSessionHandler = serviceProvider.GetRequiredService<IQueryHandler<GetSessionQuery, Session?>>();
            var session = await getSessionHandler.HandleAsync(new GetSessionQuery(sessionKey), cancellationToken);
            
            if (session == null)
            {
                throw new SessionCannotBeCreatedOrFound(sessionKey);
            }

            var ingestDriversHandler = serviceProvider.GetRequiredService<ICommandHandler<IngestDriversCommand>>();
            await ingestDriversHandler.HandleAsync(new IngestDriversCommand(sessionKey, session.Id), cancellationToken);

            var getDriversHandler = serviceProvider.GetRequiredService<IQueryHandler<GetSessionDriversQuery, IEnumerable<Driver>>>();
            var drivers = await getDriversHandler.HandleAsync(new GetSessionDriversQuery(session.Id), cancellationToken);

            var ingestLapsHandler = serviceProvider.GetRequiredService<ICommandHandler<IngestLapsCommand>>();

            foreach (var driver in drivers) // fix - old drivers
            {
                await ingestLapsHandler.HandleAsync(new IngestLapsCommand(sessionKey, session.Id, driver.DriverNumber, driver.Id), cancellationToken);
            }
            
            var ingestCarDataHandler = serviceProvider.GetRequiredService<ICommandHandler<IngestCarDataCommand>>();

            foreach (var driver in drivers)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                Log.Information("Starting car data ingestion for driver {DriverNumber}", driver.DriverNumber);

                try
                {
                    await ingestCarDataHandler.HandleAsync(new IngestCarDataCommand(sessionKey, session.Id, driver.DriverNumber, driver.Id, session.StartDate), cancellationToken);
                    Log.Information("Successfully completed car data ingestion for driver {DriverNumber}", driver.DriverNumber);
                }
                catch (Exception ex)
                {
                    Log.Warning("Failed to ingest car data for driver {DriverNumber}: {Error}", driver.DriverNumber, ex.Message);
                }
            }

            Log.Information("Completed ingestion for session {SessionKey}", sessionKey);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during session {SessionKey} ingestion", sessionKey);
            throw;
        }
    }
}