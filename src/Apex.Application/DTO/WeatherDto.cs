namespace Apex.Application.DTO;

public class WeatherDto
{
    public DateTime Date { get; set; }
    public decimal Air_Temperature { get; set; }
    public int Humidity { get; set; }
    public int Meeting_Key { get; set; }
    public decimal Pressure { get; set; }
    public int Rainfall { get; set; }
    public int Session_Key { get; set; }
    public decimal Track_Temperature { get; set; }
    public int Wind_Direction { get; set; }
    public decimal Wind_Speed { get; set; }
}
