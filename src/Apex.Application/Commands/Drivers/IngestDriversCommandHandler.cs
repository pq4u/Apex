using Apex.Application.Abstractions;
using Apex.Application.Services;
using Serilog;

namespace Apex.Application.Commands.Drivers;

public class IngestDriversCommandHandler : ICommandHandler<IngestDriversCommand>
{
    private readonly IDriverAssociationService _driverAssociationService;

    public IngestDriversCommandHandler(IDriverAssociationService driverAssociationService)
    {
        _driverAssociationService = driverAssociationService;
    }

    public async Task HandleAsync(IngestDriversCommand command)
    {
        var result = await _driverAssociationService.AssociateDriversWithSessionAsync(
            command.SessionKey, command.SessionId);

        if (!result.IsSuccess)
        {
            throw new InvalidOperationException($"Driver association failed: {result.ErrorMessage}");
        }

        Log.Information("Successfully associated {Count} drivers with session {SessionKey}", result.DriversProcessed, command.SessionKey);
    }
}