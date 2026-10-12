using Microsoft.Extensions.Logging;
using PropertyRental.Application.Common.Errors;
using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.Interfaces.Repositories;
using PropertyRental.Application.RentalManagement.Extensions;
using PropertyRental.Application.RentalManagement.Models.RentalApplication;
using PropertyRental.Domain.Entities.RentalEntities;

namespace PropertyRental.Application.RentalManagement.Query.GetRentalApplicationById;

public class GetRentalApplicationByIdQueryHandler(
    IRentalApplicationRepository rentalApplicationRepository,
    ILogger<GetRentalApplicationByIdQueryHandler> logger
) : IQueryHandler<GetRentalApplicationByIdQuery, Result<RentalApplicationModel>>
{
    private readonly IRentalApplicationRepository _rentalApplicationRepository = rentalApplicationRepository;
    private readonly ILogger<GetRentalApplicationByIdQueryHandler> _logger = logger;

    public async Task<Result<RentalApplicationModel>> ExecuteAsync(GetRentalApplicationByIdQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Starting rental application retrieval for manager. RentalApplicationId: {RentalApplicationId}",
            query.Id);

        var rentalApplication = await _rentalApplicationRepository.GetByIdWithResidencesAsync(query.Id, track: false, cancellationToken);
        if (rentalApplication is null)
        {
            _logger.LogWarning("Rental application not found. RentalApplicationId: {RentalApplicationId}", query.Id);
            return Result<RentalApplicationModel>.Fail(DomainErrors.NotFound(nameof(RentalApplication)));
        }

        _logger.LogInformation("Rental application retrieved successfully for manager. RentalApplicationId: {RentalApplicationId}", rentalApplication.Id);
        return Result<RentalApplicationModel>.Success(rentalApplication.ToRentalApplicationModel());
    }
}