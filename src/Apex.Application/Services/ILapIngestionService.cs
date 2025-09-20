using Apex.Domain.Requests;
using Apex.Domain.Results;

namespace Apex.Application.Services;

public interface ILapIngestionService
{
    Task IngestLapsAsync(LapIngestionRequest request);
}
