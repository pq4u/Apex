using Apex.Domain.Results;

namespace Apex.Domain.States;

public class TelemetryIngestionState
{
    public int ConsecutiveEmptyBatches { get; set; }
    public bool HasData { get; set; }
    public int TotalRecordsProcessed { get; set; }
    public DateTime StartTime { get; set; } = DateTime.UtcNow;
    public List<string> ProcessingNotes { get; set; } = new();
    public int ApiErrorCount { get; set; }
    public int TimeoutErrorCount { get; set; }

    public void RecordEmptyResponse()
    {
        if (HasData)
        {
            ConsecutiveEmptyBatches++;
        }
        else
        {
            ConsecutiveEmptyBatches = 0;
        }
    }

    public void RecordDataBatch(int recordCount)
    {
        HasData = true;
        ConsecutiveEmptyBatches = 0;
        TotalRecordsProcessed += recordCount;
    }

    public void RecordApiError()
    {
        ApiErrorCount++;
    }

    //public void RecordTimeoutError()
    //{
    //    TimeoutErrorCount++;
    //}

    public void RecordUnexpectedError()
    {
        throw new NotImplementedException();
    }

    public TelemetryIngestionResult ToResult()
    {
        var duration = DateTime.UtcNow - StartTime;
        return TelemetryIngestionResult.Success(TotalRecordsProcessed, duration);
    }

    public TelemetryIngestionResult ToFailedResult(string errorMessage)
        => TelemetryIngestionResult.Failed(errorMessage, TotalRecordsProcessed);
}