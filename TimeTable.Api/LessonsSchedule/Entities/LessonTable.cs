using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Common_DayOfWeek = TimeTable.Api.LessonsSchedule.Common.DayOfWeek;
using DayOfWeek = TimeTable.Api.LessonsSchedule.Common.DayOfWeek;

namespace TimeTable.Api.LessonsSchedule.Entities;

public sealed class LessonTable
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; init; } = string.Empty;

    [BsonElement("dayOfWeek")]
    public Common_DayOfWeek DayOfWeek { get; set; }

    [BsonElement("lessons")]
    public string[][] Lessons { get; set; } = [];
}