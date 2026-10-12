using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.Common.Models.Pagination;
using PropertyRental.Application.Interfaces.Queries;
using PropertyRental.Application.Interfaces.Services;
using PropertyRental.Application.RentalManagement.Models.RentalApplication;

namespace PropertyRental.Application.RentalManagement.Query.GetApplicantRentalApplications;

public class GetAllApplicantRentalApplicationsQueryHandler(
    IRentalApplicationQueries rentalApplicationQueries,
    IUserContextService userContextService
) : IQueryHandler<GetAllApplicantRentalApplicationsQuery, Result<PaginatedModel<RentalApplicationListItemModel>>>
{
    private readonly IRentalApplicationQueries _rentalApplicationQueries = rentalApplicationQueries;
    private readonly IUserContextService _userContextService = userContextService;

    public async Task<Result<PaginatedModel<RentalApplicationListItemModel>>> ExecuteAsync(GetAllApplicantRentalApplicationsQuery query, CancellationToken cancellationToken)
    {
        var user = _userContextService.Current;

        var paginated = await _rentalApplicationQueries.GetAllForApplicantFilteredAndPaginatedAsync(
            user.UserId, query.Model.Status, query.Model.PropertyId, query.Model.PageNumber, query.Model.PageSize,
            cancellationToken);

        return Result<PaginatedModel<RentalApplicationListItemModel>>.Success(paginated);
    }
}