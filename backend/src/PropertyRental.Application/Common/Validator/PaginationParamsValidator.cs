using FluentValidation;
using PropertyRental.Application.Common.Models.Pagination;

namespace PropertyRental.Application.Common.Validator;

public class PaginationParamsValidator : AbstractValidator<PaginationParams>
{
    public PaginationParamsValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);
    }
}