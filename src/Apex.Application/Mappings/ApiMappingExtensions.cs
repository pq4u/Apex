using Apex.Application.DTO.Api;
using Apex.Domain.Entities;
using Apex.Domain.TimeSeries;

namespace Apex.Application.Mappings;

public static class ApiMappingExtensions
{
    public static DriverResultDto ToResultDto(this Driver entity) => new DriverResultDto
    {
        Id = entity.Id,
        FirstName = entity.FirstName,
        LastName = entity.LastName,
        NameAcronym = entity.NameAcronym,
        DriverNumber = entity.DriverNumber,
        CountryCode = entity.CountryCode,
        HeadshotUrl = entity.HeadshotUrl
    };

    public static LapResultDto ToResultDto(this Lap entity) => new LapResultDto
    {
        Id = entity.Id,
        SessionId = entity.SessionId,
        DriverId = entity.DriverId,
        LapNumber = entity.LapNumber,
        StartDate = entity.DateStart,
        LapDurationMs = entity.LapDurationMs,
        DurationSector1Ms = entity.DurationSector1Ms,
        DurationSector2Ms = entity.DurationSector2Ms,
        DurationSector3Ms = entity.DurationSector3Ms,
        I1Speed = entity.I1Speed,
        I2Speed = entity.I2Speed,
        FinishLineSpeed = entity.FinishLineSpeed,
        StSpeed = entity.StSpeed,
        IsPitOutLap = entity.IsPitOutLap,
        SegmentsJson = entity.SegmentsJson
    };
    
    public static MeetingResultDto ToResultDto(this Meeting entity) => new MeetingResultDto
    {
        Id = entity.Id,
        Name = entity.Name,
        Location = entity.Location,
        CountryName = entity.CountryName,
        CircuitName = entity.CircuitShortName,
        StartDate = entity.DateStart
    };
    
    public static SessionResultDto ToResultDto(this Session entity) => new SessionResultDto
    {
        Id = entity.Id,
        MeetingId = entity.MeetingId,
        Name = entity.Name,
        StartDate = entity.StartDate,
        EndDate = entity.EndDate
    };
    
    public static StintResultDto ToResultDto(this Stint entity) => new StintResultDto()
    {
        SessionId = entity.SessionId,
        DriverId = entity.DriverId,
        StintNumber = entity.StintNumber,
        StartLap = entity.LapStart,
        EndLap = entity.LapEnd,
        Compound = entity.Compound,
        StartTyreAge = entity.TyreAgeAtStart
    };
    
    public static TelemetryResultDto ToResultDto(this Telemetry entity) => new TelemetryResultDto()
    {
        Time = entity.Time,
        SessionId = entity.SessionId,
        DriverId = entity.DriverId,
        Speed = entity.Speed,
        Rpm = entity.Rpm,
        Gear = entity.NGear,
        Throttle = entity.Throttle,
        Brake = entity.Brake,
        Drs = entity.Drs
    };
}