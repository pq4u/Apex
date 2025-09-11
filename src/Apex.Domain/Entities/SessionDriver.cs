namespace Apex.Domain.Entities;

public class SessionDriver
{
    public int SessionId { get; set; }
    public int DriverId { get; set; }
    public int TeamId { get; set; }

    public Session Session { get; set; }
    public Driver Driver { get; set; }
    public Team Team { get; set; }
}
