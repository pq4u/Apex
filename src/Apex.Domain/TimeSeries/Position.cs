namespace Apex.Domain.TimeSeries;

public class Position
{
    public DateTime Time { get; set; }
    public int SessionId { get; set; }
    public int DriverId { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
}
