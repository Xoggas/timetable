using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using DayOfWeek = TimeTableBackend.LessonsSchedule.Common.DayOfWeek;

namespace TimeTableBackend.LessonsSchedule.Entities;

public sealed class LessonTable
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("dayOfWeek")]
    public DayOfWeek DayOfWeek { get; set; }

    [BsonElement("lessons")]
    public string[][] Lessons { get; set; } = [];
}