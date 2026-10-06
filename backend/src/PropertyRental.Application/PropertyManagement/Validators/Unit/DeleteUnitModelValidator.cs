using FluentValidation;
using PropertyRental.Application.PropertyManagement.Models.Unit;

namespace PropertyRental.Application.PropertyManagement.Validators.Unit;

public class DeleteUnitModelValidator : AbstractValidator<DeleteUnitModel>
{
    public DeleteUnitModelValidator()
    {
        RuleFor(x => x.RowVersion)
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