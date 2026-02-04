namespace DataIngestion;

public sealed record ProviderRecord(
    string Provider,
    string Series,
    DateOnly AsOfDate,
    decimal Value);