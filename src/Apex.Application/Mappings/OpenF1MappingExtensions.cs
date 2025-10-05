using System.Text.Json;
using Apex.Application.DTO;
using Apex.Domain.Entities;
using Apex.Domain.TimeSeries;

namespace Apex.Application.Mappings;

public static class OpenF1MappingExtensions
{
    public static Meeting ToEntity(this MeetingDto dto) => new Meeting
    {
        Key = dto.MeetingKey,
        Name = dto.MeetingName,
        OfficialName = dto.MeetingOfficialName,
        Location = dto.Location,
        CountryKey = dto.CountryKey,
        CountryName = dto.CountryName,
        CircuitKey = dto.CircuitKey,
        CircuitShortName = dto.CircuitShortName,
        DateStart = dto.DateStart,
        Year = dto.Year
    };

    public static Session ToEntity(this SessionDto dto) => new Session
    {
        Key = dto.Session_Key,
        Type = dto.Session_Type,
        Name = dto.Session_Name,
        StartDate = dto.Date_Start.ToUniversalTime(),
        EndDate = dto.Date_End.ToUniversalTime(),
        GmtOffset = dto.Gmt_Offset,
        Status = dto.Session_Status
    };

    public static Driver ToEntity(this DriverDto dto) => new Driver
    {
        DriverNumber = dto.DriverNumber,
        BroadcastName = dto.BroadcastName,
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        FullName = dto.FullName,
        NameAcronym = dto.NameAcronym,
        HeadshotUrl = dto.HeadshotUrl,
        CountryCode = dto.CountryCode
    };
    
    public static Stint ToEntity(this StintDto dto, int sessionId, int driverId) => new Stint
    {
        SessionId = sessionId,
        DriverId = driverId,
        Compound = dto.Compound,
        StintNumber = dto.StintNumber,
        TyreAgeAtStart = dto.TyreAgeAtStart,
        LapStart = dto.LapStart ?? 0,
        LapEnd = dto.LapEnd ?? 0,
    };

    public static Team ExtractTeam(this DriverDto dto) => new Team
    {
        Name = dto.TeamName,
        TeamColour = dto.TeamColour
    };

    public static Lap ToEntity(this LapDto dto, int sessionId, int driverId) => new Lap
    {
        SessionId = sessionId,
        DriverId = driverId,
        LapNumber = dto.LapNumber,
        DateStart = dto.DateStart?.ToUniversalTime(),
        LapDurationMs = dto.LapDuration.HasValue ? (int)(dto.LapDuration.Value * 1000) : null,
        DurationSector1Ms = dto.DurationSector1.HasValue ? (int)(dto.DurationSector1.Value * 1000) : null,
        DurationSector2Ms = dto.DurationSector2.HasValue ? (int)(dto.DurationSector2.Value * 1000) : null,
        DurationSector3Ms = dto.DurationSector3.HasValue ? (int)(dto.DurationSector3.Value * 1000) : null,
        I1Speed = dto.I1Speed,
        I2Speed = dto.I2Speed,
        FinishLineSpeed = dto.FinishLineSpeed,
        StSpeed = dto.StSpeed,
        IsPitOutLap = dto.IsPitOutLap,
        SegmentsJson = JsonSerializer.Serialize(dto.Segments)
    };

    public static Telemetry ToTimeSeries(this CarDataDto dto, int sessionId, int driverId) => new Telemetry
    {
        Time = dto.Date.ToUniversalTime(),
        SessionId = sessionId,
        DriverId = driverId,
        Speed = (short)dto.Speed,
        Rpm = (short)dto.Rpm,
        Gear = (short)dto.Gear,
        Throttle = (short)dto.Throttle,
        Brake = (short)dto.Brake,
        Drs = (short)dto.Drs,
        NGear = (short)dto.N_Gear
    };

    public static Position ToTimeSeries(this PositionDto dto, int sessionId, int driverId) => new Position
    {
        Time = dto.Date,
        SessionId = sessionId,
        DriverId = driverId,
        X = dto.X,
        Y = dto.Y,
        Z = dto.Z
    };
}
