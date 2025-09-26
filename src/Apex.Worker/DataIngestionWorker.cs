using Apex.Application.Abstractions;
using Apex.Application.Commands.Drivers;
using Apex.Application.Commands.Laps;
using Apex.Application.Commands.Meetings;
using Apex.Application.Commands.Sessions;
using Apex.Application.Commands.Telemetry;
using Apex.Application.Queries.Drivers;
using Apex.Application.Queries.Meetings;
using Apex.Application.Queries.Races;
using Apex.Application.Queries.Sessions;
using Apex.Application.Services;
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
        Log.Information("Data ingestion worker started");
        using var scope = _serviceProvider.CreateScope();
        
        try
        {
            await IngestMeetingsAsync(scope.ServiceProvider);
                
            var meetings = await GetMeetingsAsync(scope.ServiceProvider);
            var meetingKeys = meetings?.Select(x => x.Key).ToList();
                
            await IngestSessionsAsync(scope.ServiceProvider, meetingKeys);

            var sessions = await GetSessionsAsync(scope.ServiceProvider);
            
            await IngestSessionDataAsync(scope.ServiceProvider, sessions);
                
            Log.Information("Data ingestion completed");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Data ingestion error: {ErrorMessage}", ex.Message);
        }
    }

    private async Task IngestMeetingsAsync(IServiceProvider serviceProvider)
    {
        var ingestMeetingsHandler = serviceProvider.GetRequiredService<ICommandHandler<IngestMeetingsCommand>>();
        await ingestMeetingsHandler.HandleAsync(new IngestMeetingsCommand());
    }
    
    private async Task IngestSessionsAsync(IServiceProvider serviceProvider, List<int>? meetingsKeys)
    {
        var ingestSessionsHandler = serviceProvider.GetRequiredService<ICommandHandler<IngestSessionsCommand>>();

        if (meetingsKeys is null)
            return;
        
        foreach (var meetingKey in meetingsKeys)
        {
            await ingestSessionsHandler.HandleAsync(new IngestSessionsCommand(meetingKey));
        }
    }

    private async Task<IEnumerable<Meeting>?> GetMeetingsAsync(IServiceProvider serviceProvider)
    {
        var getMeetingHandler = serviceProvider.GetRequiredService<IQueryHandler<GetMeetingsQuery, IEnumerable<Meeting>?>>();
        var meetings = await getMeetingHandler.HandleAsync(new GetMeetingsQuery());

        return meetings;
    }
    
    private async Task<IEnumerable<Session>?> GetSessionsAsync(IServiceProvider serviceProvider)
    {
        var getSessionHandler = serviceProvider.GetRequiredService<IQueryHandler<GetAllRacesQuery, IEnumerable<Session>?>>();
        var sessions = await getSessionHandler.HandleAsync(new GetAllRacesQuery());

        return sessions;
    }

    private async Task IngestSessionDataAsync(IServiceProvider serviceProvider, IEnumerable<Session>? sessions)
    {
        if (sessions is null)
            return;
        
        var currentSessionKey = -1;
        
        try
        {
            var sessionDataOrchestrator = serviceProvider.GetRequiredService<ISessionDataOrchestrator>();
            
            foreach (var session in sessions)
            {
                currentSessionKey = session.Key;
                await sessionDataOrchestrator.IngestSessionDataAsync(session.Id, session.Key, session.StartDate);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during session {SessionKey} ingestion", currentSessionKey);
            throw;
        }
    }
}