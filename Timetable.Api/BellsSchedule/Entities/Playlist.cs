using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Timetable.Api.LessonsSchedule.Common;

namespace Timetable.Api.BellsSchedule.Entities;

public sealed class Playlist
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; init; } = string.Empty;

    [BsonElement("dayOfWeek")]
    public CustomDayOfWeek DayOfWeek { get; init; }

    [BsonElement("sounds")]
    public string[] SoundFilesIds { get; init; } = [];
}