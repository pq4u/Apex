namespace Apex.Domain.Entities;

public class Driver
{
    public int Id { get; set; }
    public int DriverNumber { get; set; }
    public string BroadcastName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string FullName { get; set; }
    public string NameAcronym { get; set; }
    public string? HeadshotUrl { get; set; }
    public string? CountryCode { get; set; }

    public ICollection<SessionDriver> SessionDrivers { get; set; }
    public ICollection<Lap> Laps { get; set; }
    public ICollection<Stint> Stints { get; set; }
    public ICollection<PitStop> PitStops { get; set; }
}
