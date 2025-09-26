using Apex.Application.Client;
using Apex.Application.DTO;
using Serilog;
using System.Text.Json;

namespace Apex.Worker.Services;

public class OpenF1ApiClient : IOpenF1ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;
    private const string DateFormat = "yyyy-MM-ddTHH:mm:ss.fff";

    public OpenF1ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = OpenF1JsonOptions.Default;
    }

    public async Task<List<DriverDto>?> GetDriversAsync(int sessionKey)
    {
        var url = $"drivers?session_key={sessionKey}";
        return await FetchDataAsync<List<DriverDto>>(url!);
    }

    public async Task<List<CarDataDto>?> GetCarDataBatchAsync(int sessionKey, int driverNumber, DateTime startDate, DateTime endDate)
    {
        var start = startDate.ToString(DateFormat);
        var end = endDate.ToString(DateFormat);
        var url = $"car_data?session_key={sessionKey}&driver_number={driverNumber}&date>{start}&date<{end}";

        Log.Debug("Fetching car data: {Url}", url);
        return await FetchDataAsync<List<CarDataDto>>(url) ?? new List<CarDataDto>();
    }

    public async Task<List<SessionDto>?> GetSessionsAsync(int meetingKey)
    {
        var url = $"sessions?meeting_key={meetingKey}";
        var sessions = await FetchDataAsync<List<SessionDto>>(url);
        return sessions;
    }

    public async Task<List<MeetingDto>?> GetMeetingsAsync()
    {
        var url = $"meetings";
        var meetings = await FetchDataAsync<List<MeetingDto>>(url);
        return meetings;
    }

    public async Task<List<LapDto>?> GetLapsAsync(int meetingKey, int driverNumber)
    {
        var url = $"laps?session_key={meetingKey}&driver_number={driverNumber}";
        var laps = await FetchDataAsync<List<LapDto>>(url);
        return laps;
    }

    private async Task<T?> FetchDataAsync<T>(string endpoint)
    {
        try
        {
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error fetching data from {Endpoint}", endpoint);
            throw;
        }
    }
}
