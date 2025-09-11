namespace Apex.Application.DTO;

public class SessionDto
{
    public int Session_Key { get; set; }
    public string Session_Name { get; set; }
    public int Meeting_Key { get; set; }
    public string Session_Type { get; set; }
    public DateTime Date_Start { get; set; }
    public DateTime Date_End { get; set; }
    public string Gmt_Offset { get; set; }
    public string Location { get; set; }
    public int Country_Key { get; set; }
    public string Country_Code { get; set; }
    public string Country_Name { get; set; }
    public int Circuit_Key { get; set; }
    public string Circuit_Short_Name { get; set; }
    public string Session_Status { get; set; }
    public int Year { get; set; }
}
