using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.Common.Models.Pagination;
using PropertyRental.Application.Interfaces.Queries;
using PropertyRental.Application.RentalManagement.Models.RentalApplication;

namespace PropertyRental.Application.RentalManagement.Query.GetAllRentalApplications;

public class GetAllRentalApplicationsQueryHandler(
    IRentalApplicationQueries rentalApplicationQueries
) : IQueryHandler<GetAllRentalApplicationsQuery, Result<PaginatedModel<RentalApplicationListItemModel>>>
{
    private readonly IRentalApplicationQueries _rentalApplicationQueries = rentalApplicationQueries;

    public async Task<Result<PaginatedModel<RentalApplicationListItemModel>>> ExecuteAsync(GetAllRentalApplicationsQuery query, CancellationToken cancellationToken)
    {
        var paginated = await _rentalApplicationQueries
            .GetAllFilteredAndPaginatedAsync(query.Model.Status, query.Model.PropertyId, query.Model.PageNumber, query.Model.PageSize, cancellationToken);
        
        return Result<PaginatedModel<RentalApplicationListItemModel>>.Success(paginated);
    }
}