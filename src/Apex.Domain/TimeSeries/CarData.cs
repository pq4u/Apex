namespace Apex.Domain.TimeSeries;

public class TelemetryData
{
    public DateTime Time { get; set; }
    public int SessionId { get; set; }
    public int DriverId { get; set; }
    public short Speed { get; set; }
    public short Rpm { get; set; }
    public short Gear { get; set; }
    public short Throttle { get; set; }
    public short Brake { get; set; }
    public short Drs { get; set; }
    public short NGear { get; set; }
}