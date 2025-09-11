namespace Apex.Domain.Requests;

public record LapIngestionRequest(int SessionKey, int SessionId, int DriverNumber, int DriverId);
