using System.Text.Json.Serialization;

namespace Apex.Application.DTO;

public class RaceControlDto
{
    public string Category { get; set; }
    public DateTime Date { get; set; }
    
    public string Description { get; set; }
    
    [JsonPropertyName("Driver_Number")]
    public int? DriverNumber { get; set; }
    public string Flag { get; set; }
    
    [JsonPropertyName("Lap_Number")]
    public int? LapNumber { get; set; }
    
    [JsonPropertyName("Meeting_Key")]
    public int MeetingKey { get; set; }
    
    public string Message { get; set; }
    public string Scope { get; set; }
    public string Sector { get; set; }
    
    [JsonPropertyName("Session_Key")]
    public int Session_Key { get; set; }
}
