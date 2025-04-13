using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace Timetable.Api.Shared.Validation;

public sealed class MongoIdAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        return value is string id && (string.IsNullOrEmpty(id) || ObjectId.TryParse(id, out _));
    }
}