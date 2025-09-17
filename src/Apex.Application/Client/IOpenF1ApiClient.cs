using Apex.Application.DTO;

namespace Apex.Application.Client;

public interface IOpenF1ApiClient
{
    Task<List<DriverDto>> GetDriversAsync(int sessionKey);
    Task<List<CarDataDto>> GetCarDataBatchAsync(int sessionKey, int driverNumber, DateTime startDate, DateTime endDate);
    Task<SessionDto?> GetSessionAsync(int sessionKey);
    Task<List<MeetingDto>?> GetMeetingsAsync();
    Task<List<LapDto>?> GetLapsAsync(int meetingKey, int driverNumber);
}