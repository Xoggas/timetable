using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Timetable.Api.BellsSchedule.Entities;

public sealed class ManualEvent
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("event_name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("sound_file_id")]
    public string SoundFileId { get; set; } = string.Empty;
}