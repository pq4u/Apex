namespace Apex.Domain.Entities;

public class Driver
{
    public int Id { get; set; }
    public int DriverNumber { get; set; }
    public string BroadcastName { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string FullName { get; set; } = null!;
    public string NameAcronym { get; set; } = null!;
    public string? HeadshotUrl { get; set; }
    public string? CountryCode { get; set; }

    public ICollection<SessionDriver> SessionDrivers { get; set; } = null!;
    public ICollection<Lap> Laps { get; set; } = null!;
    public ICollection<Stint> Stints { get; set; } = null!;
    public ICollection<PitStop> PitStops { get; set; } = null!;
}
