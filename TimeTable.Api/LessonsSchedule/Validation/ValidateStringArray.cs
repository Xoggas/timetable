using System.ComponentModel.DataAnnotations;

namespace TimeTable.Api.LessonsSchedule.Validation;

public sealed class ValidateStringArrayAttribute : ValidationAttribute
{
    private readonly int _stringLength;

    public ValidateStringArrayAttribute(int stringLength)
    {
        _stringLength = stringLength;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string[][] stringArray)
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