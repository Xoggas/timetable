using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TimeTableBackend.LessonsSchedule.Entities;

public sealed class Lesson
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    
    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;
}