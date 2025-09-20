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
        await _driverAssociationService.AssociateDriversWithSessionAsync(command.SessionKey, command.SessionId);
        
        Log.Information("Successfully associated drivers with session {SessionKey}", command.SessionKey);
    }
}