using System.Text.Json.Serialization;

namespace Apex.Application.DTO;

public class StintDto
{
    [JsonPropertyName("Driver_Number")]
    public int DriverNumber { get; set; }
    
    [JsonPropertyName("Lap_Start")]
    public int? LapStart { get; set; }
    
    [JsonPropertyName("Lap_End")]
    public int? LapEnd { get; set; }
    
    [JsonPropertyName("Meeting_Key")]
    public int MeetingKey { get; set; }
    
    [JsonPropertyName("Session_Key")]
    public int SessionKey { get; set; }
    
    [JsonPropertyName("Stint_Number")]
    public int StintNumber { get; set; }
    public string Compound { get; set; }
    
    [JsonPropertyName("Tyre_Age_At_Start")]
    public int TyreAgeAtStart { get; set; }
}
