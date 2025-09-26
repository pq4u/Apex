namespace Apex.Application.Services;

public interface ISessionDataOrchestrator
{
    Task IngestSessionDataAsync(int sessionId, int sessionKey, DateTime sessionStartDate);
}