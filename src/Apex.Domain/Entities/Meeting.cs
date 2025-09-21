namespace Apex.Domain.Entities;

public class Meeting
{
    public int Id { get; set; }
    public int Key { get; set; }
    public string Name { get; set; }
    public string OfficialName { get; set; }
    public string Location { get; set; }
    public int CountryKey { get; set; }
    public string CountryName { get; set; }
    public int CircuitKey { get; set; }
    public string CircuitShortName { get; set; }
    public DateTime DateStart { get; set; }
    public int Year { get; set; }

    public ICollection<Session> Sessions { get; set; }
}
