using System.Text.Json.Serialization;

namespace Apex.Application.DTO.Ingestion;

public class PitDto
{
    public DateTime Date { get; set; }
    
    [JsonPropertyName("Driver_Number")]
    public int DriverNumber { get; set; }
    
    [JsonPropertyName("Lap_Number")]
    public int LapNumber { get; set; }
    
    [JsonPropertyName("Meeting_Key")]
    public int MeetingKey { get; set; }
    
    [JsonPropertyName("Pit_Duration")]
    public double PitDuration { get; set; }
    
    [JsonPropertyName("Session_Key")]
    public int SessionKey { get; set; }
}
