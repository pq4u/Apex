namespace Apex.Domain.Entities;

public class RaceControl
{
    public long Id { get; set; }
    public int SessionId { get; set; }
    public DateTime Date { get; set; }
    public int? LapNumber { get; set; }
    public string Category { get; set; }
    public string Flag { get; set; }
    public string Message { get; set; }
    public int? DriverId { get; set; }

    public Session Session { get; set; }
    public Driver Driver { get; set; }
}
