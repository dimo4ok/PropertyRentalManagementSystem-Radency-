using FluentValidation;
using PropertyRental.Application.PropertyManagement.Models;

namespace PropertyRental.Application.PropertyManagement.Validators;

public class DeletePropertyModelValidator : AbstractValidator<DeletePropertyModel>
{
    public DeletePropertyModelValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

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