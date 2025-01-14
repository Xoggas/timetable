using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TimeTableBackend.LessonsSchedule.Entities;

public sealed class Lesson
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; init; } = string.Empty;

    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
}