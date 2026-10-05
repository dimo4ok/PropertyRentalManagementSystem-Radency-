using Microsoft.AspNetCore.Http;
using PropertyRental.Application.Common.Errors;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.RentalManagement.Interfaces;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.RentalManagement.Services;

public class RentalApplicationService(
    IRentalApplicationRepository rentalApplicationRepository) : IRentalApplicationService
{
    private readonly IRentalApplicationRepository _rentalApplicationRepository =
        rentalApplicationRepository;

    public async Task<Result> ValidateCanBeDeletedAsync(IEnumerable<Unit> units, CancellationToken cancellationToken)
    {
        var unitIds = units
            .Select(x => x.Id)
            .ToHashSet();
        if (unitIds.Count == 0) return Result.Success();

        var applications = await _rentalApplicationRepository.GetAllAsync(cancellationToken);

        var hasApplications = applications.Any(x => unitIds.Contains(x.UnitId));
        if (hasApplications)
            return Result.Fail(RentalApplicationErrors.UnitHasApplications, StatusCodes.Status400BadRequest);

        return Result.Success();
    }
}