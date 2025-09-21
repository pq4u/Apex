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
                await IngestMeetingsAsync(scope.ServiceProvider);
                
                var meetings = await GetMeetingsAsync(scope.ServiceProvider);
                var meetingKeys = meetings?.Select(x => x.Key).ToList();

                
                await IngestSessionsAsync(scope.ServiceProvider, meetingKeys);

                var sessions = await GetSessionsAsync(scope.ServiceProvider);
                
                await IngestSessionDataAsync(scope.ServiceProvider, sessions);
                
                
                //Log.Information("Successfully ingested session {SessionKey}", sessionKey);
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "Failed to ingest session {SessionKey}", sessionKey);
            }
        }
    }

    private async Task IngestMeetingsAsync(IServiceProvider serviceProvider)
    {
        Log.Information("Starting meeting list ingestion from OpenF1 API");
        
        var ingestMeetingsHandler = serviceProvider.GetRequiredService<ICommandHandler<IngestMeetingsCommand>>();
        await ingestMeetingsHandler.HandleAsync(new IngestMeetingsCommand());

        Log.Information("Completed meeting list ingestion from OpenF1 API");
    }
    
    private async Task IngestSessionsAsync(IServiceProvider serviceProvider, List<int>? meetingsKeys)
    {
        Log.Information("Starting session list ingestion from OpenF1 API");
        
        var ingestSessionsHandler = serviceProvider.GetRequiredService<ICommandHandler<IngestSessionsCommand>>();

        foreach (var meetingKey in meetingsKeys)
        {
            await ingestSessionsHandler.HandleAsync(new IngestSessionsCommand(meetingKey));
        }
        
        Log.Information("Completed session list ingestion from OpenF1 API");
    }

    private async Task<IEnumerable<Meeting>?> GetMeetingsAsync(IServiceProvider serviceProvider)
    {
        var getMeetingHandler = serviceProvider.GetRequiredService<IQueryHandler<GetMeetingsQuery, IEnumerable<Meeting>?>>();
        var meetings = await getMeetingHandler.HandleAsync(new GetMeetingsQuery());

        return meetings;
    }
    
    private async Task<IEnumerable<Session>?> GetSessionsAsync(IServiceProvider serviceProvider)
    {
        var getSessionHandler = serviceProvider.GetRequiredService<IQueryHandler<GetSessionsQuery, IEnumerable<Session>?>>();
        var sessions = await getSessionHandler.HandleAsync(new GetSessionsQuery());

        return sessions;
    }

    private async Task IngestSessionDataAsync(IServiceProvider serviceProvider, IEnumerable<Session> sessions)
    {
        Log.Information("Starting data ingestion for {SessionKey}", sessions.Count());;

        var currentSessionKey = -1;
        
        try
        {
            var ingestDriversHandler = serviceProvider.GetRequiredService<ICommandHandler<IngestDriversCommand>>();
            var getDriversSessionHandler = serviceProvider.GetRequiredService<IQueryHandler<GetSessionDriversQuery, IEnumerable<Driver>>>();
            var ingestLapsHandler = serviceProvider.GetRequiredService<ICommandHandler<IngestLapsCommand>>();
            var ingestCarDataHandler = serviceProvider.GetRequiredService<ICommandHandler<IngestCarDataCommand>>();

            foreach (var session in sessions)
            {
                currentSessionKey = session.Key;
                await ingestDriversHandler.HandleAsync(new IngestDriversCommand(session.Id, session.Key));

                var drivers = await getDriversSessionHandler.HandleAsync(new GetSessionDriversQuery(session.Id));


                foreach (var driver in drivers)
                {
                    await ingestLapsHandler.HandleAsync(new IngestLapsCommand(session.Key, session.Id, driver.DriverNumber, driver.Id));
                }
                

                foreach (var driver in drivers)
                {
                    Log.Information("Starting car data ingestion for driver {DriverNumber}", driver.DriverNumber);

                    try
                    {
                        await ingestCarDataHandler.HandleAsync(new IngestCarDataCommand(session.Key, session.Id, driver.DriverNumber, driver.Id, session.StartDate));
                        Log.Information("Successfully completed car data ingestion for driver {DriverNumber}", driver.DriverNumber);
                    }
                    catch (Exception ex)
                    {
                        Log.Warning("Failed to ingest car data for driver {DriverNumber}: {Error}", driver.DriverNumber, ex.Message);
                    }
                }

                Log.Information("Completed data ingestion for session {SessionKey}", session.Key);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during session {SessionKey} ingestion", currentSessionKey);
            throw;
        }
    }
}