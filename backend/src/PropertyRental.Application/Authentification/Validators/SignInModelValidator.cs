using FluentValidation;
using PropertyRental.Application.Authentification.Models;

namespace PropertyRental.Application.Authentification.Validators;

public class SignInModelValidator : AbstractValidator<SignInModel>
{
    public SignInModelValidator()
    {
        RuleFor(x => x.UserName)
            .MinimumLength(3)
            .MaximumLength(20);

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}