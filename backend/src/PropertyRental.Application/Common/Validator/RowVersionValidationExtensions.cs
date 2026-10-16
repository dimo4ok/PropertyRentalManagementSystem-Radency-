using FluentValidation;

namespace PropertyRental.Application.Common.Validator;

public static class RowVersionValidationExtensions
{
    public static IRuleBuilderOptions<T, string> ValidRowVersion<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .Must(BeValidBase64)
            .WithMessage("RowVersion must be a valid Base64 string.");
    }

    private static bool BeValidBase64(string value)
    {
        try
        {
            Convert.FromBase64String(value);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}