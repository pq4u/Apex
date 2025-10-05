using System.Text.Json.Serialization;

namespace Apex.Application.DTO;

public class MeetingDto
{
    
    [JsonPropertyName("Meeting_Key")]
    public int MeetingKey { get; set; }
    
    [JsonPropertyName("Meeting_Name")]
    public string MeetingName { get; set; }
    
    [JsonPropertyName("Meeting_Official_Name")]
    public string MeetingOfficialName { get; set; }
    
    [JsonPropertyName("Location")]
    public string Location { get; set; }
    
    [JsonPropertyName("Country_Key")]
    public int CountryKey { get; set; }
    
    [JsonPropertyName("Country_Code")]
    public string CountryCode { get; set; }
    
    [JsonPropertyName("Country_Name")]
    public string CountryName { get; set; }
    
    [JsonPropertyName("Circuit_Key")]
    public int CircuitKey { get; set; }
    
    [JsonPropertyName("Circuit_Short_Name")]
    public string CircuitShortName { get; set; }
    
    [JsonPropertyName("Date_Start")]
    public DateTime DateStart { get; set; }
    
    [JsonPropertyName("Gmt_Offset")]
    public string GmtOffset { get; set; }
    public int Year { get; set; }
}
