using System.Text.Json.Serialization;

namespace Apex.Application.DTO;

public class CarDataDto
{
    public DateTime Date { get; set; }
    
    [JsonPropertyName("Driver_Number")]
    public int DriverNumber { get; set; }
    
    [JsonPropertyName("Lap_Number")]
    public int LapNumber { get; set; }
    
    [JsonPropertyName("Meeting_Key")]
    public int MeetingKey { get; set; }
    
    [JsonPropertyName("Session_Key")]
    public int SessionKey { get; set; }
    public int Speed { get; set; }
    public int Rpm { get; set; }
    public int Gear { get; set; }
    public int Throttle { get; set; }
    public int Brake { get; set; }
    public int Drs { get; set; }
    public int N_Gear { get; set; }
}
