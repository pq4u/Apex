namespace Apex.Domain.Entities;

public class Team
{
    public int Id { get; set; }
    public int Key { get; set; }
    public string Name { get; set; } = null!;
    public string TeamColour { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
