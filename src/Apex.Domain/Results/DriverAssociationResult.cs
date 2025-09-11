namespace Apex.Domain.Results;

public class DriverAssociationResult
{
    public bool IsSuccess { get; private set; }
    public Dictionary<int, int> DriverNumberToIdMapping { get; private set; } = new();
    public string? ErrorMessage { get; private set; }
    public int DriversProcessed { get; private set; }

    private DriverAssociationResult() { }

    public static DriverAssociationResult Success(Dictionary<int, int> driverMapping)
        => new() 
        { 
            IsSuccess = true, 
            DriverNumberToIdMapping = driverMapping,
            DriversProcessed = driverMapping.Count
        };

    public static DriverAssociationResult Failed(string errorMessage, int processedCount = 0)
        => new() 
        { 
            IsSuccess = false, 
            ErrorMessage = errorMessage,
            DriversProcessed = processedCount
        };
}
