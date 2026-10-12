using FluentValidation;
using PropertyRental.Application.Common.Validator;
using PropertyRental.Application.PropertyManagement.Models;
using PropertyRental.Application.PropertyManagement.Models.Property;

namespace PropertyRental.Application.PropertyManagement.Validators.Property;

public class DeletePropertyModelValidator : AbstractValidator<DeletePropertyModel>
{
    public DeletePropertyModelValidator()
    {
        RuleFor(x => x.RowVersion) 
            .ValidRowVersion();
    }
}