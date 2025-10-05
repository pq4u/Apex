using System.Text.Json.Serialization;

namespace Apex.Application.DTO;

public class WeatherDto
{
    public DateTime Date { get; set; }
    
    [JsonPropertyName("Air_Temperature")]
    public decimal AirTemperature { get; set; }
    public int Humidity { get; set; }
    
    [JsonPropertyName("Meeting_Key")]
    public int MeetingKey { get; set; }
    public decimal Pressure { get; set; }
    public int Rainfall { get; set; }
    
    [JsonPropertyName("Session_Key")]
    public int SessionKey { get; set; }
    
    [JsonPropertyName("Track_Temperature")]
    public decimal TrackTemperature { get; set; }
    
    [JsonPropertyName("Wind_Direction")]
    public int WindDirection { get; set; }
    
    [JsonPropertyName("Wind_Speed")]
    public decimal WindSpeed { get; set; }
}
