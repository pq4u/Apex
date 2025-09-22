namespace Apex.Domain.Exceptions;

public class LapsNotFoundForDriverInSessionInApiException : CustomException
{
    public int DriverNumber { get; }
    public int SessionKey { get; }
    public LapsNotFoundForDriverInSessionInApiException(int driverNumber, int sessionKey)
        : base($"Laps not found for driver number {driverNumber} in session key {sessionKey}")
    {
        DriverNumber = driverNumber;
        SessionKey = sessionKey;
    }
}