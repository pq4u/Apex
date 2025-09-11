namespace Apex.Domain.Entities;

public class PitStop
{
    public long Id { get; set; }
    public int SessionId { get; set; }
    public int DriverId { get; set; }
    public int LapNumber { get; set; }
    public DateTime Date { get; set; }
    public decimal PitDuration { get; set; }

    public Session Session { get; set; }
    public Driver Driver { get; set; }
}
