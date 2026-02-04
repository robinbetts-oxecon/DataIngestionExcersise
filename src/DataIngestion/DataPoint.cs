namespace DataIngestion;

public sealed record DataPoint(
    string Provider,
    string Series,
    DateOnly AsOfDate,
    decimal Value,
    DateTimeOffset IngestedAtUtc)
{
    public DataPointKey Key => new(Provider, Series, AsOfDate);
}

public readonly record struct DataPointKey(string Provider, string Series, DateOnly AsOfDate);