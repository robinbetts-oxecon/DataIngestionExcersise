using NUnit.Framework;

namespace DataIngestion.Tests;

public sealed class IngestionServiceTests
{
    private InMemoryDataPointRepository _repo = null!;
    private InMemoryBatchTracker _tracker = null!;
    private FakeClock _clock = null!;
    private IngestionService _serviceUnderTest = null!;

    [SetUp]
    public void SetUp()
    {
        _repo = new InMemoryDataPointRepository();
        _tracker = new InMemoryBatchTracker();
        _clock = new FakeClock(DateTimeOffset.Parse("2026-01-01T00:00:00Z"));
        _serviceUnderTest = new IngestionService(_repo, _tracker, _clock);
    }

    [Test]
    public async Task Should_deduplicate_within_batch_by_business_key_keeping_last()
    {
        var batchId = "providerB-2026-01-01";
        var records = new[]
        {
            new ProviderRecord("ProviderB", "GDP", new DateOnly(2026, 1, 1), 100m),
            new ProviderRecord("ProviderB", "GDP", new DateOnly(2026, 1, 1), 101m), // duplicate key, later wins
            new ProviderRecord("ProviderB", "GDP", new DateOnly(2026, 1, 2), 102m),
        };

        await _serviceUnderTest.IngestAsync(batchId, records);

        var all = await _repo.ReadAllAsync(CancellationToken.None);
        Assert.That(all.Count, Is.EqualTo(2), "Duplicates inside a batch should be collapsed by business key.");
        var key = new DataPointKey("ProviderB", "GDP", new DateOnly(2026, 1, 1));
        Assert.That(all[key].Value, Is.EqualTo(101m), "The last record in the batch should win for a duplicate key.");
    }

    private sealed class FakeClock : IClock
    {
        public FakeClock(DateTimeOffset now) => UtcNow = now;
        public DateTimeOffset UtcNow { get; }
    }
}
