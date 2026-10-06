using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PropertyRental.Application.Common.Errors;
using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.Interfaces;
using PropertyRental.Application.PropertyManagement.Interfaces;
using PropertyRental.Application.RentalManagement.Interfaces;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.PropertyManagement.Commands.DeleteUnit;

public class DeleteUnitCommandHandler(
    IUnitRepository unitRepository,
    IRentalApplicationService rentalApplicationService,
    IUnitOfWork unitOfWork,
    ILogger<DeleteUnitCommandHandler> logger)
    : ICommandHandler<DeleteUnitCommand, Result>
{
    private readonly IUnitRepository _unitRepository = unitRepository;
    private readonly IRentalApplicationService _rentalApplicationService = rentalApplicationService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<DeleteUnitCommandHandler> _logger = logger;

    public async Task<Result> ExecuteAsync(DeleteUnitCommand command, CancellationToken cancellationToken)
    {
        var unit = await _unitRepository.GetByIdAsync(command.Id, true, cancellationToken);
        if (unit is null)
        {
            _logger.LogWarning("Unit {UnitId} was not found.", command.Id);
            return Result.Fail(DomainErrors.NotFound(nameof(Unit)), StatusCodes.Status404NotFound);
        }

        var validationResult = await _rentalApplicationService.ValidateCanBeDeletedAsync(unit.Id, cancellationToken);
        if (!validationResult.IsSuccess)
            return validationResult;

        var rowVersion = Convert.FromBase64String(command.Model.RowVersion);
        _unitRepository.SetOriginalRowVersion(unit, rowVersion);

        _unitRepository.Delete(unit);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Unit {UnitId} deleted successfully.", unit.Id);
        return Result.Success(StatusCodes.Status204NoContent);
    }
}