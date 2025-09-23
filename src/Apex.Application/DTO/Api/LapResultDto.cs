namespace Apex.Application.DTO.Api;

public class LapResultDto
{
    public long Id { get; set; }
    public int SessionId { get; set; }
    public int DriverId { get; set; }
    public int LapNumber { get; set; }
    public DateTime? DateStart { get; set; }
    public int? LapDurationMs { get; set; }
    public int? DurationSector1Ms { get; set; }
    public int? DurationSector2Ms { get; set; }
    public int? DurationSector3Ms { get; set; }
    public int? I1Speed { get; set; }
    public int? I2Speed { get; set; }
    public int? FinishLineSpeed { get; set; }
    public int? StSpeed { get; set; }
    public bool IsPitOutLap { get; set; }
    public string? SegmentsJson { get; set; }
}