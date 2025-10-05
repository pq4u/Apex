namespace Apex.Domain.Entities;

public class Session
{
    public int Id { get; set; }
    public int MeetingId { get; set; }
    public int Key { get; set; }
    public string Type { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string GmtOffset { get; set; } = null!;
    public string? Status { get; set; }

    public Meeting Meeting { get; set; } = null!;
    public ICollection<SessionDriver>? SessionDrivers { get; set; }
    public ICollection<Lap>? Laps { get; set; }
    public ICollection<Stint>? Stints { get; set; }
    public ICollection<PitStop>? PitStops { get; set; }
    public ICollection<RaceControl>? RaceControls { get; set; }
    public ICollection<IngestionStatus>? IngestionStatuses { get; set; }
}
