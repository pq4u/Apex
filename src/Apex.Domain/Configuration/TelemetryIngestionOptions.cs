namespace Apex.Domain.Configuration;

public class IngestionOptions
{
    public const string SectionName = "Ingestion";
    public int MaxEmptyBatches { get; set; } = 3;
    public int ApiDelayMs { get; set; } = 500;
    public int BatchIntervalMinutes { get; set; } = 5;
    public int BatchSize { get; set; } = 10000;
}