namespace DataIngestion;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}