namespace Apex.Domain.Results;

public class TelemetryIngestionResult
{
    public bool IsSuccess { get; private set; }
    public int TotalRecordsProcessed { get; private set; }
    public string? ErrorMessage { get; private set; }
    public TimeSpan ProcessingDuration { get; private set; }

    private TelemetryIngestionResult() { }

    public static TelemetryIngestionResult Success(int totalRecords, TimeSpan duration)
        => new() 
        { 
            IsSuccess = true, 
            TotalRecordsProcessed = totalRecords,
            ProcessingDuration = duration
        };

    public static TelemetryIngestionResult Failed(string errorMessage, int processedSoFar = 0)
        => new() 
        { 
            IsSuccess = false, 
            ErrorMessage = errorMessage,
            TotalRecordsProcessed = processedSoFar
        };
}
