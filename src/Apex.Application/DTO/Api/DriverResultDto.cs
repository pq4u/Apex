namespace Apex.Application.DTO.Api;

public class DriverResultDto
{
    public int Id { get; set; }
    public int DriverNumber { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string NameAcronym { get; set; } = null!;
    public string? HeadshotUrl { get; set; }
    public string CountryCode { get; set; } = null!;
}