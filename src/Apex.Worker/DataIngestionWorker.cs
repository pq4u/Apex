using Apex.Application.Abstractions;
using Apex.Application.Commands.Drivers;
using Apex.Application.Commands.Laps;
using Apex.Application.Commands.Meetings;
using Apex.Application.Commands.Sessions;
using Apex.Application.Commands.Telemetry;
using Apex.Application.Queries.Drivers;
using Apex.Application.Queries.Meetings;
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

        // to do: refactor!
        
        using (var scope = _serviceProvider.CreateScope())
        {
            try
            {
                await IngestMeetingsAsync(scope.ServiceProvider, stoppingToken);
                
                var meetings = await GetMeetingsAsync(scope.ServiceProvider, stoppingToken);
                var meetingKeys = meetings?.Select(x => x.Key).ToList();

                
                await IngestSessionsAsync(scope.ServiceProvider, meetingKeys, stoppingToken);

                var sessions = await GetSessionsAsync(scope.ServiceProvider, stoppingToken);
                
                
                await IngestSessionDataAsync(scope.ServiceProvider, sessions, stoppingToken);
                
                
                //Log.Information("Successfully ingested session {SessionKey}", sessionKey);
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "Failed to ingest session {SessionKey}", sessionKey);
            }
        }
    }

    private async Task IngestMeetingsAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        Log.Information("Starting meeting list ingestion from OpenF1 API");
        
        var ingestMeetingsHandler = serviceProvider.GetRequiredService<ICommandHandler<IngestMeetingsCommand>>();
        await ingestMeetingsHandler.HandleAsync(new IngestMeetingsCommand(), cancellationToken);

        Log.Information("Completed meeting list ingestion from OpenF1 API");
    }
    
    private async Task IngestSessionsAsync(IServiceProvider serviceProvider, List<int>? meetingsKeys, CancellationToken cancellationToken)
    {
        Log.Information("Starting session list ingestion from OpenF1 API");
        
        var ingestSessionsHandler = serviceProvider.GetRequiredService<ICommandHandler<IngestSessionsCommand>>();

        foreach (var meetingKey in meetingsKeys)
        {
            await ingestSessionsHandler.HandleAsync(new IngestSessionsCommand(meetingKey), cancellationToken);
        }
        
        Log.Information("Completed session list ingestion from OpenF1 API");
    }

    private async Task<List<Meeting>?> GetMeetingsAsync(IServiceProvider serviceProvider,
        CancellationToken cancellationToken)
    {
        var getMeetingHandler = serviceProvider.GetRequiredService<IQueryHandler<GetMeetingsQuery, List<Meeting>?>>();
        var meetings = await getMeetingHandler.HandleAsync(new GetMeetingsQuery(), cancellationToken);

        return meetings;
    }
    
    private async Task<List<Session>?> GetSessionsAsync(IServiceProvider serviceProvider,
        CancellationToken cancellationToken)
    {
        var getSessionHandler = serviceProvider.GetRequiredService<IQueryHandler<GetSessionsQuery, List<Session>?>>();
        var sessions = await getSessionHandler.HandleAsync(new GetSessionsQuery(), cancellationToken);

        return sessions;
    }

    private async Task IngestSessionDataAsync(IServiceProvider serviceProvider, List<Session> sessions, CancellationToken cancellationToken)
    {
        Log.Information("Starting data ingestion for {SessionKey}", sessions.Count());;

        try
        {
            var ingestDriversHandler = serviceProvider.GetRequiredService<ICommandHandler<IngestDriversCommand>>();
            var getDriversHandler = serviceProvider.GetRequiredService<IQueryHandler<GetSessionDriversQuery, IEnumerable<Driver>>>();
            var ingestLapsHandler = serviceProvider.GetRequiredService<ICommandHandler<IngestLapsCommand>>();
            var ingestCarDataHandler = serviceProvider.GetRequiredService<ICommandHandler<IngestCarDataCommand>>();

            foreach (var session in sessions)
            {
                await ingestDriversHandler.HandleAsync(new IngestDriversCommand(session.Key, session.Id), cancellationToken);

                var drivers = await getDriversHandler.HandleAsync(new GetSessionDriversQuery(session.Id), cancellationToken);


                foreach (var driver in drivers) // fix - old drivers
                {
                    await ingestLapsHandler.HandleAsync(new IngestLapsCommand(session.Key, session.Id, driver.DriverNumber, driver.Id), cancellationToken);
                }
                

                foreach (var driver in drivers)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    Log.Information("Starting car data ingestion for driver {DriverNumber}", driver.DriverNumber);

                    try
                    {
                        await ingestCarDataHandler.HandleAsync(new IngestCarDataCommand(session.Key, session.Id, driver.DriverNumber, driver.Id, session.StartDate), cancellationToken);
                        Log.Information("Successfully completed car data ingestion for driver {DriverNumber}", driver.DriverNumber);
                    }
                    catch (Exception ex)
                    {
                        Log.Warning("Failed to ingest car data for driver {DriverNumber}: {Error}", driver.DriverNumber, ex.Message);
                    }
                }

                Log.Information("Completed ingestion for session {SessionKey}", session.Key);
            }
        }
        catch (Exception ex)
        {
            //Log.Error(ex, "Error during session {SessionKey} ingestion", session.Key);
            throw;
        }
    }
}