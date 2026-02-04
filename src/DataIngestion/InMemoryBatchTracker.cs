using System.Collections.Concurrent;

namespace DataIngestion;

public sealed class InMemoryBatchTracker : IBatchTracker
{
    private readonly ConcurrentDictionary<string, byte> _processed = new();

    public Task<bool> HasProcessedAsync(string batchId, CancellationToken ct)
        => Task.FromResult(_processed.ContainsKey(batchId));

    public Task MarkProcessedAsync(string batchId, CancellationToken ct)
    {
        _processed.TryAdd(batchId, 0);
        return Task.CompletedTask;
    }
}