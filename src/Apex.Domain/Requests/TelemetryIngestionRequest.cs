namespace Apex.Domain.Requests;

public record TelemetryIngestionRequest(int SessionKey, int SessionId, int DriverNumber, int DriverId, DateTime StartDate);
