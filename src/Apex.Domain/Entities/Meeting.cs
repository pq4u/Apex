namespace Apex.Domain.Entities;

public class Meeting
{
    public int Id { get; set; }
    public int Key { get; set; }
    public string Name { get; set; } = null!;
    public string OfficialName { get; set; } = null!;
    public string Location { get; set; } = null!;
    public int CountryKey { get; set; }
    public string CountryName { get; set; } = null!;
    public int CircuitKey { get; set; }
    public string CircuitShortName { get; set; } = null!;
    public DateTime DateStart { get; set; }
    public int Year { get; set; }

    public ICollection<Session> Sessions { get; set; } = null!;
}
