using FluentValidation;
using PropertyRental.Application.PropertyManagement.Models;
using PropertyRental.Application.PropertyManagement.Models.Property;

namespace PropertyRental.Application.PropertyManagement.Validators.Property;

public class DeletePropertyModelValidator : AbstractValidator<DeletePropertyModel>
{
    public DeletePropertyModelValidator()
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