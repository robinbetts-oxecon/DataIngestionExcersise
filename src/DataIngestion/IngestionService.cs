namespace DataIngestion;

/// <summary>
/// Ingests provider records into the repository.
///
/// NOTE: This class is intentionally imperfect for interview purposes.
/// </summary>
public sealed class IngestionService
{
    private readonly IDataPointRepository _repo;
    private readonly IBatchTracker _batchTracker;
    private readonly IClock _clock;

    public IngestionService(IDataPointRepository repo, IBatchTracker batchTracker, IClock clock)
    {
        _repo = repo;
        _batchTracker = batchTracker;
        _clock = clock;
    }

    public async Task IngestAsync(string batchId, IReadOnlyCollection<ProviderRecord> records, CancellationToken ct = default)
    {
        if (await _batchTracker.HasProcessedAsync(batchId, ct))
        {
            return;
        }

        foreach (var r in records)
        {
            var point = new DataPoint(
                Provider: r.Provider,
                Series: r.Series,
                AsOfDate: r.AsOfDate,
                Value: r.Value,
                IngestedAtUtc: _clock.UtcNow);

            await _repo.UpsertManyAsync([point], ct);
        }
    }
}
