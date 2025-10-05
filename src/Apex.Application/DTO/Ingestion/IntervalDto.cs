using System.Text.Json.Serialization;

namespace Apex.Application.DTO;

public class IntervalDto
{
    public DateTime Date { get; set; }
    
    [JsonPropertyName("Driver_Number")]
    public int DriverNumber { get; set; }
    
    [JsonPropertyName("Gap_To_Leader")]
    public double? GapToLeader { get; set; }
    public double? Interval { get; set; }
    
    [JsonPropertyName("Meeting_Key")]
    public int MeetingKey { get; set; }
    
    [JsonPropertyName("Session_Key")]
    public int SessionKey { get; set; }
}
