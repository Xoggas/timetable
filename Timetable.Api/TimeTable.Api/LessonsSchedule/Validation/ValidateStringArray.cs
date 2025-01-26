using System.ComponentModel.DataAnnotations;

namespace Timetable.Api.LessonsSchedule.Validation;

public sealed class MinMaxLengthAttribute : ValidationAttribute
{
    private readonly int _minLength;
    private readonly int _maxLength;

    public MinMaxLengthAttribute(int minLength, int maxLength)
    {
        _minLength = minLength;
        _maxLength = maxLength;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string?[][] stringArray)
        {
            return new ValidationResult("Value is not a string array");
        }

        if (stringArray.SelectMany(x => x).Any(y => y is null || y.Length < _minLength || y.Length > _maxLength))
        {
            return new ValidationResult($"String length mustn't be greater than {_maxLength}");
        }

        return ValidationResult.Success;
    }
}