using Apex.Domain.Results;

namespace Apex.Application.Services;

public interface IDriverAssociationService
{
    Task AssociateDriversWithSessionAsync(int sessionKey, int sessionId);
}