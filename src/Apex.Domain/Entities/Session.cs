namespace Apex.Domain.Entities;

public class Session
{
    public int Id { get; set; }
    public int MeetingId { get; set; }
    public int Key { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
    public DateTime? DateStart { get; set; }
    public DateTime? DateEnd { get; set; }
    public string GmtOffset { get; set; }
    public string? Status { get; set; }

    public Meeting Meeting { get; set; }
    public ICollection<SessionDriver>? SessionDrivers { get; set; }
    public ICollection<Lap>? Laps { get; set; }
    public ICollection<Stint>? Stints { get; set; }
    public ICollection<PitStop>? PitStops { get; set; }
    public ICollection<RaceControl>? RaceControls { get; set; }
    public ICollection<IngestionStatus>? IngestionStatuses { get; set; }
}
