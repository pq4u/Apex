namespace Apex.Application.DTO;

public class IntervalDto
{
    public DateTime Date { get; set; }
    public int Driver_Number { get; set; }
    public double? Gap_To_Leader { get; set; }
    public double? Interval { get; set; }
    public int Meeting_Key { get; set; }
    public int Session_Key { get; set; }
}
