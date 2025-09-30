using Apex.Application.DTO.Api;
using Apex.Domain.Entities;

namespace Apex.Application.Mappings;

public static class ApiMappingExtensions
{
    public static DriverResultDto ToEntity(this Driver driver) => new DriverResultDto
    {
        Id = driver.Id,
        FirstName = driver.FirstName,
        LastName = driver.LastName,
        NameAcronym = driver.NameAcronym,
        DriverNumber = driver.DriverNumber,
        CountryCode = driver.CountryCode,
        HeadshotUrl = driver.HeadshotUrl
    };
}