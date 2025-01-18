namespace TimeTable.Api.Shared;

public sealed class MongoDbSettings
{
    public string ConnectionString { get; init; } = string.Empty;
    public string DatabaseName { get; init; } = string.Empty;
}