using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Timetable.Api.BellsSchedule.Entities;

public sealed class BellTable
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; init; } = string.Empty;

    [Required]
    public BellTableRow[] Rows { get; init; } = [];
}

public sealed class BellTableRow
{
    [BsonElement("start_sound_id")]
    public string StartSoundId { get; init; } = string.Empty;

    [BsonElement("start_time")]
    public TimeEntity StartTime { get; init; }

    [BsonElement("end_sound_id")]
    public string EndSoundId { get; init; } = string.Empty;

    [BsonElement("end_time")]
    public TimeEntity EndTime { get; init; }
}