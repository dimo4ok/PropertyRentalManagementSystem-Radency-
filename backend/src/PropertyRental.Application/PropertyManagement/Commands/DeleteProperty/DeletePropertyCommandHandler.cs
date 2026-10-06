using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PropertyRental.Application.Common.Errors;
using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.Interfaces;
using PropertyRental.Application.PropertyManagement.Interfaces;
using PropertyRental.Application.RentalManagement.Interfaces;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.PropertyManagement.Commands.DeleteProperty;

public class DeletePropertyCommandHandler(
    IPropertyRepository propertyRepository,
    IRentalApplicationService rentalApplicationService,
    IUnitOfWork unitOfWork,
    ILogger<DeletePropertyCommandHandler> logger)
    : ICommandHandler<DeletePropertyCommand, Result>
{
    private readonly IPropertyRepository _propertyRepository = propertyRepository;
    private readonly IRentalApplicationService _rentalApplicationService = rentalApplicationService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<DeletePropertyCommandHandler> _logger = logger;

    public async Task<Result> ExecuteAsync(DeletePropertyCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting property with id {PropertyId}.", command.Id);

        var property = await _propertyRepository.GetByIdWithUnitsAsync(command.Id, true, cancellationToken);
        if (property is null)
        {
            _logger.LogWarning("Property with id {PropertyId} was not found.", command.Id);
            return Result.Fail(DomainErrors.NotFound(nameof(Property)));
        }

        var unitIds = property.Units.Select(x => x.Id);
        var validationResult = await _rentalApplicationService.ValidateCanBeDeletedAsync(unitIds, cancellationToken);

        if (!validationResult.IsSuccess)
        {
            _logger.LogWarning(
                "Property {PropertyId} cannot be deleted because one or more units have rental applications.",
                property.Id);
            return validationResult;
        }

        var rowVersion = Convert.FromBase64String(command.Model.RowVersion);
        _propertyRepository.SetOriginalRowVersion(property, rowVersion);

        _propertyRepository.Delete(property);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Property with id {PropertyId} deleted successfully.", property.Id);
        return Result.Success(StatusCodes.Status204NoContent);
    }
}