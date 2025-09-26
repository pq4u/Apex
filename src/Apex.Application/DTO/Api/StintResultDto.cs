namespace Apex.Application.DTO.Api;

public class StintResultDto
{
    public int SessionId { get; set; }
    public int DriverId { get; set; }
    public int StartLap { get; set; }
    public int EndLap { get; set; }
    public int StintNumber { get; set; }
    public string Compound { get; set; }
    public int? StartTyreAge { get; set; }
}