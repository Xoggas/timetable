using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Timetable.Api.LessonsSchedule.Common;

namespace Timetable.Api.LessonsSchedule.Entities;

public sealed class LessonTable
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonIgnore]
    public string Id { get; init; } = string.Empty;

    [BsonElement("dayOfWeek")]
    public CustomDayOfWeek DayOfWeek { get; set; }

    [BsonElement("lessons")]
    public string[][] Lessons { get; set; } = [];
}