namespace Apex.Domain.Entities;

public class Stint
{
    public long Id { get; set; }
    public int SessionId { get; set; }
    public int DriverId { get; set; }
    public int StintNumber { get; set; }
    public int LapStart { get; set; }
    public int LapEnd { get; set; }
    public int? TyreAgeAtStart { get; set; }
    public string Compound { get; set; }

    public Session Session { get; set; }
    public Driver Driver { get; set; }
}
