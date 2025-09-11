using Apex.Application.Abstractions;
using Apex.Application.Services;
using Apex.Domain.Requests;
using Serilog;

namespace Apex.Application.Commands.Laps;

public class IngestLapsCommandHandler : ICommandHandler<IngestLapsCommand>
{
    private readonly ILapService _lapService;

    public IngestLapsCommandHandler(ILapService lapService)
    {
        _lapService = lapService;
    }

    public async Task HandleAsync(IngestLapsCommand command, CancellationToken cancellationToken = default)
    {
        var request = new LapIngestionRequest(command.SessionKey, command.SessionId, command.DriverNumber, command.DriverId);
        var result = await _lapService.IngestLapsAsync(request, cancellationToken);

        if (!result.IsSuccess)
        {
            throw new InvalidOperationException($"Lap ingestion failed: {result.ErrorMessage}");
        }

        if (result.AlreadyExisted)
        {
            Log.Information("Laps of driver {DriverNumber} in session {SessionKey} already existed",
                command.DriverNumber, command.SessionKey);
        }
        else
        {
            Log.Information("Successfully ingested lap of driver {DriverNumber} in session {SessionKey}", 
                command.DriverNumber, command.SessionKey);
        }
    }
}
