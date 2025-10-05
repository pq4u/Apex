using System.Text.Json.Serialization;

namespace Apex.Application.DTO;

public class SessionDto
{
    [JsonPropertyName("Session_Key")]
    public int Session_Key { get; set; }
    
    [JsonPropertyName("Session_Name")]
    public string Session_Name { get; set; }
    
    [JsonPropertyName("Meeting_Key")]
    public int Meeting_Key { get; set; }
    
    [JsonPropertyName("Session_Type")]
    public string Session_Type { get; set; }
    
    [JsonPropertyName("Date_Start")]
    public DateTime Date_Start { get; set; }
    
    [JsonPropertyName("Date_End")]
    public DateTime Date_End { get; set; }
    
    [JsonPropertyName("Gmt_Offset")]
    public string Gmt_Offset { get; set; }
    public string Location { get; set; }
    
    [JsonPropertyName("Country_Key")]
    public int Country_Key { get; set; }
    
    [JsonPropertyName("Country_Code")]
    public string Country_Code { get; set; }
    
    [JsonPropertyName("Country_Name")]
    public string Country_Name { get; set; }
    
    [JsonPropertyName("Circuit_Key")]
    public int Circuit_Key { get; set; }
    
    [JsonPropertyName("Circuit_Short_Name")]
    public string Circuit_Short_Name { get; set; }
    
    [JsonPropertyName("Session_Status")]
    public string Session_Status { get; set; }
    public int Year { get; set; }
}
