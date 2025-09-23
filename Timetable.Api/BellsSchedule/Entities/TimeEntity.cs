using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Timetable.Api.BellsSchedule.Entities;

public readonly struct TimeEntity
{
    [JsonIgnore]
    public int TotalMinutes => Hour * 60 + Minute;

    [BsonElement("hour")]
    public int Hour { get; init; }

    [BsonElement("minute")]
    public int Minute { get; init; }
}