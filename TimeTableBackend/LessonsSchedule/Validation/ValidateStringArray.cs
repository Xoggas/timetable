using System.ComponentModel.DataAnnotations;

namespace TimeTableBackend.LessonsSchedule.Validation;

public sealed class ValidateStringArrayAttribute : ValidationAttribute
{
    private readonly int _stringLength;

    public ValidateStringArrayAttribute(int stringLength)
    {
        _stringLength = stringLength;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var stringArray = value as string[][];

        if (stringArray is null)
        {
            return new ValidationResult("Value is not a string array");
        }

        if (stringArray.SelectMany(x => x).Any(y => y.Length > _stringLength))
        {
            return new ValidationResult($"String length mustn't be greater than {_stringLength}");
        }

        return ValidationResult.Success;
    }
}