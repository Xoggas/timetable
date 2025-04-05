using MongoDB.Bson.Serialization.Attributes;

namespace Timetable.Api.BellsSchedule.Entities;

public readonly struct TimeEntity
{
    [BsonElement("hour")]
    public int Hour { get; init; }
    
    [BsonElement("minute")]
    public int Minute { get; init; }
}