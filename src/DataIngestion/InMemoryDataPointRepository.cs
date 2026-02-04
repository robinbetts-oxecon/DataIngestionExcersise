using System.Collections.Concurrent;

namespace DataIngestion;

public sealed class InMemoryDataPointRepository : IDataPointRepository
{
    private readonly ConcurrentDictionary<DataPointKey, DataPoint> _store = new();
    private int _upsertCalls;

    public int UpsertManyCallCount => _upsertCalls;

    public Task UpsertManyAsync(IReadOnlyCollection<DataPoint> points, CancellationToken ct)
    {
        Interlocked.Increment(ref _upsertCalls);

        foreach (var p in points.Select(x => x).OrderBy(x => x.Value))
        {
            _store[p.Key] = p; // upsert by business key
        }

        return Task.CompletedTask;
    }

    public Task<IReadOnlyDictionary<DataPointKey, DataPoint>> ReadAllAsync(CancellationToken ct)
        => Task.FromResult((IReadOnlyDictionary<DataPointKey, DataPoint>)_store);
}
