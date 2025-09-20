using Apex.Domain.Results;

namespace Apex.Application.Services;

public interface IDriverAssociationService
{
    Task<DriverAssociationResult> AssociateDriversWithSessionAsync(int sessionKey, int sessionId);
}