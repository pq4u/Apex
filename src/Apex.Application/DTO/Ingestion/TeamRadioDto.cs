using System.Text.Json.Serialization;

namespace Apex.Application.DTO;

public class TeamRadioDto
{
    public DateTime Date { get; set; }
    
    [JsonPropertyName("Driver_Number")]
    public int DriverNumber { get; set; }
    
    [JsonPropertyName("Meeting_Key")]
    public int MeetingKey { get; set; }
    
    [JsonPropertyName("Recording_Url")]
    public string RecordingUrl { get; set; }
    
    [JsonPropertyName("Session_Key")]
    public int SessionKey { get; set; }
}
