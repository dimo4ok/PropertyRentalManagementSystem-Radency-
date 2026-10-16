using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PropertyRental.Application.Common.Errors;
using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.Interfaces;
using PropertyRental.Application.Interfaces.Queries;
using PropertyRental.Application.Interfaces.Repositories;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.PropertyManagement.Commands.DeleteUnit;

public class DeleteUnitCommandHandler(
    IUnitRepository unitRepository,
    IRentalApplicationQueries rentalApplicationQueries,
    IUnitOfWork unitOfWork,
    ILogger<DeleteUnitCommandHandler> logger)
    : ICommandHandler<DeleteUnitCommand, Result>
{
    private readonly IUnitRepository _unitRepository = unitRepository;
    private readonly IRentalApplicationQueries _rentalApplicationQueries = rentalApplicationQueries;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<DeleteUnitCommandHandler> _logger = logger;

    public async Task<Result> ExecuteAsync(DeleteUnitCommand command, CancellationToken cancellationToken)
    {
        var unit = await _unitRepository.GetByIdWithUnitTypesAsync(command.Id, true, cancellationToken);
        if (unit is null)
        {
            _logger.LogWarning("Unit {UnitId} was not found.", command.Id);
            return Result.Fail(DomainErrors.NotFound(nameof(Unit)), StatusCodes.Status404NotFound);
        }

        var hasApplicationForUnit = await _rentalApplicationQueries.HasApplicationForUnitAsync(unit.Id, cancellationToken);
        if (hasApplicationForUnit)
        {
            _logger.LogWarning( "Unit deletion failed. Unit has rental applications. UnitId: {UnitId}", unit.Id);
            return Result.Fail(RentalApplicationErrors.UnitHasApplications, StatusCodes.Status400BadRequest);
        }

        var rowVersion = Convert.FromBase64String(command.Model.RowVersion);
        _unitRepository.SetOriginalRowVersion(unit, rowVersion);

        _unitRepository.Delete(unit);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Unit {UnitId} deleted successfully.", unit.Id);
        return Result.Success(StatusCodes.Status204NoContent);
    }
}