namespace Apex.Application.DTO.Api;

public class SessionResultDto
{
    public int Id { get; set; }
    public int MeetingId { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}