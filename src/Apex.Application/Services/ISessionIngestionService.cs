using Apex.Application.DTO;
using Apex.Domain.Requests;

namespace Apex.Application.Services;

public interface ISessionIngestionService
{
    Task<List<SessionDto>> IngestSessionsAsync(SessionIngestionRequest request);
}

