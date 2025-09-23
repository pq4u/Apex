namespace Apex.Application.DTO;

public class LapDto
{
    public DateTime? Date_Start { get; set; }
    public int Driver_Number { get; set; }
    public float? Duration_Sector_1 { get; set; }
    public float? Duration_Sector_2 { get; set; }
    public float? Duration_Sector_3 { get; set; }
    public int? I1_Speed { get; set; }
    public int? I2_Speed { get; set; }
    public bool Is_Pit_Out_Lap { get; set; }
    public float? Lap_Duration { get; set; }
    public int Lap_Number { get; set; }
    public int Meeting_Key { get; set; }
    public int Session_Key { get; set; }
    public int? Finish_Line_Speed { get; set; }
    public int? St_Speed { get; set; }
    public List<int> Segments { get; set; }
}
