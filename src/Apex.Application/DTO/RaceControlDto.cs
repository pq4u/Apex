namespace Apex.Application.DTO;

public class RaceControlDto
{
    public string Category { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public int? Driver_Number { get; set; }
    public string Flag { get; set; }
    public int? Lap_Number { get; set; }
    public int Meeting_Key { get; set; }
    public string Message { get; set; }
    public string Scope { get; set; }
    public string Sector { get; set; }
    public int Session_Key { get; set; }
}
