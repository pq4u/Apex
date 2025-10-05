using System.Text.Json.Serialization;

namespace Apex.Application.DTO;

public class LocationDto
{
    public DateTime Date { get; set; }
    
    [JsonPropertyName("Driver_Number")]
    public int DriverNumber { get; set; }
    
    [JsonPropertyName("Meeting_Key")]
    public int MeetingKey { get; set; }
    
    [JsonPropertyName("Session_Key")]
    public int SessionKey { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
}
