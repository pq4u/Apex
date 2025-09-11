using Apex.Domain.Results;

namespace Apex.Application.Services;

public interface ISessionManagementService
{
    Task<SessionCreationResult> EnsureSessionExistsAsync(int sessionKey, CancellationToken cancellationToken = default);
}