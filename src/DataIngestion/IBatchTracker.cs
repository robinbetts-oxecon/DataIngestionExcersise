namespace DataIngestion;

public interface IBatchTracker
{
    Task<bool> HasProcessedAsync(string batchId, CancellationToken ct);
    Task MarkProcessedAsync(string batchId, CancellationToken ct);
}