using FluentValidation;
using PropertyRental.Application.RentalManagement.Models.RentalApplication;

namespace PropertyRental.Application.RentalManagement.Validator;

public class GetAllRentalApplicationsModelValidator
    : AbstractValidator<GetAllRentalApplicationsModel>
{
    public GetAllRentalApplicationsModelValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum()
            .When(x => x.Status.HasValue);

        RuleFor(x => x.PageNumber)
            .GreaterThan(0);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);
    }
}