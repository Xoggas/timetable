using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TimeTableBackend.LessonsSchedule.Models;

public sealed class Lesson
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; } = null!;
}