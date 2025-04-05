using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Timetable.Api.BellsSchedule.Entities;

public sealed class AutomaticEvent
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; init; } = string.Empty;

    [BsonElement("is_enabled")]
    public bool IsEnabled { get; init; } = true;

    [BsonElement("event_name")]
    public string Name { get; init; } = string.Empty;

    [BsonElement("time")]
    public TimeEntity ActivationTime { get; init; }

    [BsonElement("sound_file_id")]
    public string SoundFileId { get; init; } = string.Empty;
}