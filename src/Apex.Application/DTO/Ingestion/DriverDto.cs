using System.Text.Json.Serialization;

namespace Apex.Application.DTO;

public class DriverDto
{
    [JsonPropertyName("Driver_Number")]
    public int DriverNumber { get; set; }
    
    [JsonPropertyName("Session_Key")]
    public int SessionKey { get; set; }
    
    [JsonPropertyName("Meeting_Key")]
    public int MeetingKey { get; set; }
    
    [JsonPropertyName("Broadcast_Name")]
    public string BroadcastName { get; set; } = null!;
    
    [JsonPropertyName("Country_Code")]
    public string CountryCode { get; set; } = null!;
    
    [JsonPropertyName("First_Name")]
    public string FirstName { get; set; } = null!;
    
    [JsonPropertyName("Last_Name")]
    public string LastName { get; set; } = null!;
    
    [JsonPropertyName("Full_Name")]
    public string FullName { get; set; } = null!;
    
    [JsonPropertyName("Headshot_Url")]
    public string HeadshotUrl { get; set; } = null!;
    
    [JsonPropertyName("Name_Acronym")]
    public string NameAcronym { get; set; } = null!;
    
    [JsonPropertyName("Team_Colour")]
    public string TeamColour { get; set; } = null!;
    
    [JsonPropertyName("Team_Name")]
    public string TeamName { get; set; } = null!;
}
