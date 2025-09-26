namespace Apex.Application.DTO;

public class StintDto
{
    public int Driver_Number { get; set; }
    public int? Lap_End { get; set; }
    public int? Lap_Start { get; set; }
    public int Meeting_Key { get; set; }
    public int Session_Key { get; set; }
    public int Stint_Number { get; set; }
    public string Compound { get; set; }
    public int Tyre_Age_At_Start { get; set; }
}
