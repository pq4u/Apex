namespace Apex.Domain.Entities;

public class SessionDriver
{
    public int SessionId { get; set; }
    public int DriverId { get; set; }
    public int TeamId { get; set; }

    public Session Session { get; set; } = null!;
    public Driver Driver { get; set; } = null!;
    public Team Team { get; set; } = null!;
}
