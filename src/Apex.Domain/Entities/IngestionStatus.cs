namespace Apex.Domain.Entities;

public class IngestionStatus
{
    public int Id { get; set; }
    public int SessionId { get; set; }
    public string DataType { get; set; } = null!;
    public DateTime? LastUpdated { get; set; }
    public long RecordsCount { get; set; }
    public string Status { get; set; } = null!;
    public string? ErrorMessage { get; set; }

    public Session Session { get; set; } = null!;
}
