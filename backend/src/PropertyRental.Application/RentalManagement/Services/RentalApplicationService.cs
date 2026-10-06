using Microsoft.AspNetCore.Http;
using PropertyRental.Application.Common.Errors;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.RentalManagement.Interfaces;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.RentalManagement.Services;

public class RentalApplicationService(IRentalApplicationRepository rentalApplicationRepository)
    : IRentalApplicationService
{
    private readonly IRentalApplicationRepository _rentalApplicationRepository = rentalApplicationRepository;

    public async Task<Result> ValidateCanBeDeletedAsync(Guid unitId, CancellationToken cancellationToken)
    {
        var applications = await _rentalApplicationRepository.GetAllAsync(cancellationToken);

        var hasApplication = applications.Any(x => x.UnitId == unitId);
        if (hasApplication)
            return Result.Fail(RentalApplicationErrors.UnitHasApplications, StatusCodes.Status400BadRequest);

        return Result.Success();
    }

    public async Task<Result> ValidateCanBeDeletedAsync(IEnumerable<Guid> unitIds, CancellationToken cancellationToken)
    {
        var applications = await _rentalApplicationRepository.GetAllAsync(cancellationToken);
        var hasApplication = applications.Any(x => unitIds.Contains(x.UnitId));

        if (hasApplication)
            return Result.Fail(RentalApplicationErrors.UnitHasApplications, StatusCodes.Status400BadRequest);

        return Result.Success();
    }
}