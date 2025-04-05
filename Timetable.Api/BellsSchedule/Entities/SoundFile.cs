using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Timetable.Api.BellsSchedule.Entities;

public sealed class SoundFile
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string FileId { get; init; } = string.Empty;
    
    [BsonElement("file_name")]
    public string FileName { get; init; } = string.Empty;
}