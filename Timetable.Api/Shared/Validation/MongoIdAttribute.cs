using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace Timetable.Api.Shared.Validation;

public sealed class MongoIdAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        return value switch
        {
            string id => ValidateSingleId(id),
            string[] ids => ids.All(ValidateSingleId),
            _ => throw new ValidationException($"The constraint isn't applicable for {value?.GetType()}")
        };
    }

    private bool ValidateSingleId(string id)
    {
        return string.IsNullOrEmpty(id) || ObjectId.TryParse(id, out _);
    }
}