using FluentValidation;
using PropertyRental.Application.Common.Validator;
using PropertyRental.Application.RentalManagement.Models.RentalApplication;

namespace PropertyRental.Application.RentalManagement.Validator;

public class ApproveRentalApplicationModelValidator : AbstractValidator<ApproveRentalApplicationModel>
{
    public ApproveRentalApplicationModelValidator()
    {
        RuleFor(x => x.RowVersion).ValidRowVersion();
    }
}