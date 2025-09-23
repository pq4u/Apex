using Apex.Application.DTO;
using Apex.Domain.Entities;
using Apex.Domain.TimeSeries;

namespace Apex.Application.Mappings;

public static class OpenF1MappingExtensions
{
    public static Meeting ToEntity(this MeetingDto dto) => new Meeting
    {
        Key = dto.Meeting_Key,
        Name = dto.Meeting_Name,
        OfficialName = dto.Meeting_Official_Name,
        Location = dto.Location,
        CountryKey = dto.Country_Key,
        CountryName = dto.Country_Name,
        CircuitKey = dto.Circuit_Key,
        CircuitShortName = dto.Circuit_Short_Name,
        DateStart = dto.Date_Start,
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
        DriverNumber = dto.Driver_Number,
        BroadcastName = dto.Broadcast_Name,
        FirstName = dto.First_Name,
        LastName = dto.Last_Name,
        FullName = dto.Full_Name,
        NameAcronym = dto.Name_Acronym,
        HeadshotUrl = dto.Headshot_Url,
        CountryCode = dto.Country_Code
    };

    public static Team ExtractTeam(this DriverDto dto) => new Team
    {
        Name = dto.Team_Name,
        TeamColour = dto.Team_Colour
    };

    public static Lap ToEntity(this LapDto dto, int sessionId, int driverId) => new Lap
    {
        SessionId = sessionId,
        DriverId = driverId,
        LapNumber = dto.Lap_Number,
        DateStart = dto.Date_Start != null ? ((DateTime)dto.Date_Start).ToUniversalTime() : null,
        LapDurationMs = dto.Lap_Duration.HasValue ? (int)(dto.Lap_Duration.Value * 1000) : null,
        DurationSector1Ms = dto.Duration_Sector_1.HasValue ? (int)(dto.Duration_Sector_1.Value * 1000) : null,
        DurationSector2Ms = dto.Duration_Sector_2.HasValue ? (int)(dto.Duration_Sector_2.Value * 1000) : null,
        DurationSector3Ms = dto.Duration_Sector_3.HasValue ? (int)(dto.Duration_Sector_3.Value * 1000) : null,
        I1Speed = dto.I1_Speed,
        I2Speed = dto.I2_Speed,
        FinishLineSpeed = dto.Finish_Line_Speed,
        StSpeed = dto.St_Speed,
        IsPitOutLap = dto.Is_Pit_Out_Lap,
        SegmentsJson = dto.Segments != null ? System.Text.Json.JsonSerializer.Serialize(dto.Segments) : null
    };

    public static TelemetryData ToTimeSeries(this CarDataDto dto, int sessionId, int driverId) => new TelemetryData
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
