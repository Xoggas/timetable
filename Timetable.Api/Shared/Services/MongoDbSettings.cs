namespace Timetable.Api.Shared.Services;

public sealed class MongoDbSettings
{
    public string ConnectionString { get; init; } = string.Empty;
    public string DatabaseName { get; init; } = string.Empty;
}