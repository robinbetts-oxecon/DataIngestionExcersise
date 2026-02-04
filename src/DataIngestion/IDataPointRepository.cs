namespace DataIngestion;

public interface IDataPointRepository
{
    /// <summary>
    /// Bulk upsert: insert new datapoints; overwrite existing datapoints with the same business key.
    /// </summary>
    Task UpsertManyAsync(IReadOnlyCollection<DataPoint> points, CancellationToken ct);

    /// <summary>
    /// Read current state (for tests / debugging).
    /// </summary>
    Task<IReadOnlyDictionary<DataPointKey, DataPoint>> ReadAllAsync(CancellationToken ct);

    /// <summary>
    /// For verifying behaviour in tests.
    /// </summary>
    int UpsertManyCallCount { get; }
}