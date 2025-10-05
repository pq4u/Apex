using System.Text.Json.Serialization;

namespace Apex.Application.DTO;

public class LapDto
{
    [JsonPropertyName("Date_Start")]
    public DateTime? DateStart { get; set; }
    
    [JsonPropertyName("Driver_Number")]
    public int DriverNumber { get; set; }
    
    [JsonPropertyName("Duration_Sector_1")]
    public float? DurationSector1 { get; set; }
    
    [JsonPropertyName("Duration_Sector_2")]
    public float? DurationSector2 { get; set; }
    
    [JsonPropertyName("Duration_Sector_3")]
    public float? DurationSector3 { get; set; }
    
    [JsonPropertyName("I1_Speed")]
    public int? I1Speed { get; set; }
    
    [JsonPropertyName("I2_Speed")]
    public int? I2Speed { get; set; }
    
    [JsonPropertyName("Is_Pit_Out_Lap")]
    public bool IsPitOutLap { get; set; }
    
    [JsonPropertyName("Lap_Duration")]
    public float? LapDuration { get; set; }
    
    [JsonPropertyName("Lap_Number")]
    public int LapNumber { get; set; }
    
    [JsonPropertyName("Session_Key")]
    public int SessionKey { get; set; }
    
    [JsonPropertyName("Finish_Line_Speed")]
    public int? FinishLineSpeed { get; set; }
    
    [JsonPropertyName("St_Speed")]
    public int? StSpeed { get; set; }
    
    [JsonPropertyName("Segments")]
    public List<int> Segments { get; set; } = new();
}
