using Apex.Application.Abstractions;
using Apex.Application.Commands.Drivers;
using Apex.Application.Commands.Laps;
using Apex.Application.Commands.SessionDrivers;
using Apex.Application.Commands.Stints;
using Apex.Application.Commands.Telemetry;
using Apex.Application.Queries.Drivers;
using Apex.Domain.Entities;
using Serilog;

namespace Apex.Application.Services;

public class SessionDataOrchestrator : ISessionDataOrchestrator
{
    private readonly ICommandHandler<IngestDriversCommand> _ingestDriversHandler;
    private readonly ICommandHandler<IngestLapsCommand> _ingestLapsHandler;
    private readonly ICommandHandler<IngestCarDataCommand> _ingestCarDataHandler;
    private readonly ICommandHandler<IngestStintsCommand> _ingestStintsHandler;
    private readonly ICommandHandler<CreateSessionDriversCommand> _createSessionDriversHandler;
    private readonly IQueryHandler<GetSessionDriversQuery, IEnumerable<Driver>?> _getDriversSessionHandler;

    public SessionDataOrchestrator(ICommandHandler<IngestDriversCommand> ingestDriversHandler,
        ICommandHandler<IngestStintsCommand> ingestStintsHandler,
        ICommandHandler<CreateSessionDriversCommand> createSessionDriversHandler,
        ICommandHandler<IngestLapsCommand> ingestLapsHandler,
        ICommandHandler<IngestCarDataCommand> ingestCarDataHandler,
        IQueryHandler<GetSessionDriversQuery, IEnumerable<Driver>?> getDriversSessionHandler)
    {
        _ingestDriversHandler = ingestDriversHandler;
        _ingestStintsHandler = ingestStintsHandler;
        _ingestLapsHandler = ingestLapsHandler;
        _ingestCarDataHandler = ingestCarDataHandler;
        _createSessionDriversHandler = createSessionDriversHandler;
        _getDriversSessionHandler = getDriversSessionHandler;
    }

    public async Task IngestSessionDataAsync(int sessionId, int sessionKey, DateTime sessionStartDate)
    {
        try
        {
            Log.Information("Starting session data ingestion for session {SessionId} with key {SessionKey}",
                sessionId, sessionKey);

            await _ingestDriversHandler.HandleAsync(new IngestDriversCommand(sessionKey));

            await _createSessionDriversHandler.HandleAsync(new CreateSessionDriversCommand(sessionId, sessionKey));
            
            var drivers = await _getDriversSessionHandler.HandleAsync(new GetSessionDriversQuery(sessionId));

            await _ingestStintsHandler.HandleAsync(new IngestStintsCommand(sessionId, sessionKey));
            
            foreach (var driver in drivers)
            {
                await _ingestLapsHandler.HandleAsync(new IngestLapsCommand(sessionKey, sessionId, driver.DriverNumber, driver.Id));
            }

            foreach (var driver in drivers)
            {
                Log.Information("Starting car data ingestion for driver {DriverNumber}", driver.DriverNumber);
            
                try
                {
                    await _ingestCarDataHandler.HandleAsync(new IngestCarDataCommand(sessionKey, sessionId, driver.DriverNumber, driver.Id, sessionStartDate));
                    Log.Information("Successfully completed car data ingestion for driver {DriverNumber}", driver.DriverNumber);
                }
                catch (Exception ex)
                {
                    Log.Error("Failed to ingest car data for driver {DriverNumber}: {Error}", driver.DriverNumber, ex.Message);
                }
            }
            
            Log.Information("Completed session data ingestion for session key {SessionKey}", sessionKey);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed session data ingestion for session key {SessionKey}", sessionKey);
            throw;
        }
    }
}