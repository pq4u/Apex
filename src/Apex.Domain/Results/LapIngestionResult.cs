namespace Apex.Domain.Results;

public class LapIngestionResult
{
    public bool IsSuccess { get; private set; }
    public int ProcessedDriver { get; private set; }
    public string? ErrorMessage { get; private set; }
    public bool AlreadyExisted { get; private set; }

    public static LapIngestionResult AlreadyExists(int driverNumber)
        => new() { IsSuccess = true, ProcessedDriver = driverNumber, AlreadyExisted = true };

    public static LapIngestionResult Success(int driverNumber)
        => new() { IsSuccess = true, ProcessedDriver = driverNumber };

    public static LapIngestionResult Failed(int driverNumber, string errorMessage)
        => new() { IsSuccess = false, ProcessedDriver = driverNumber, ErrorMessage = errorMessage };
}
