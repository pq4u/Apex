namespace Apex.Domain.Entities;

public class RaceControl
{
    public long Id { get; set; }
    public int SessionId { get; set; }
    public DateTime Date { get; set; }
    public int? LapNumber { get; set; }
    public string Category { get; set; } = null!;
    public string Flag { get; set; } = null!;
    public string Message { get; set; } = null!;
    public int? DriverId { get; set; }

    public Session Session { get; set; } = null!;
    public Driver Driver { get; set; } = null!;
}
