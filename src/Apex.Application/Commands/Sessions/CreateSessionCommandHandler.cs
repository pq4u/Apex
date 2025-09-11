using Apex.Application.Abstractions;
using Apex.Application.Services;
using Serilog;

namespace Apex.Application.Commands.Sessions;

public class CreateSessionCommandHandler : ICommandHandler<CreateSessionCommand>
{
    private readonly ISessionManagementService _sessionManagementService;

    public CreateSessionCommandHandler(ISessionManagementService sessionManagementService)
    {
        _sessionManagementService = sessionManagementService;
    }

    public async Task HandleAsync(CreateSessionCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _sessionManagementService.EnsureSessionExistsAsync(command.SessionKey, cancellationToken);

        if (!result.IsSuccess)
        {
            throw new InvalidOperationException($"Session creation failed: {result.ErrorMessage}");
        }

        if (result.AlreadyExisted)
        {
            Log.Information("Session {SessionKey} already existed", command.SessionKey);
        }
        else
        {
            Log.Information("Successfully created session {SessionKey}", command.SessionKey);
        }
    }
}