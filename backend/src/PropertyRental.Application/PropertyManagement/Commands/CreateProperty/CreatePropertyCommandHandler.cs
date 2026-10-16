using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PropertyRental.Application.Common.Errors;
using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.Interfaces;
using PropertyRental.Application.Interfaces.Queries;
using PropertyRental.Application.Interfaces.Repositories;
using PropertyRental.Application.PropertyManagement.Extensions;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.PropertyManagement.Commands.CreateProperty;

public class CreatePropertyCommandHandler(
    IPropertyRepository propertyRepository,
    IPropertyQueries propertyQueries,
    IUnitOfWork unitOfWork,
    ILogger<CreatePropertyCommandHandler> logger)
    : ICommandHandler<CreatePropertyCommand, Result>
{
    private readonly IPropertyRepository _propertyRepository = propertyRepository;
    private readonly IPropertyQueries _propertyQueries = propertyQueries;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<CreatePropertyCommandHandler> _logger = logger;

    public async Task<Result> ExecuteAsync(CreatePropertyCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating property {PropertyName}.", command.Model.Name);

        if (await _propertyQueries.ExistsAsync(command.Model.Name, command.Model.Address, cancellationToken))
        {
            _logger.LogWarning(
                "Property with name {PropertyName} and address {PropertyAddress} already exists.", command.Model.Name,
                command.Model.Address);

            return Result.Fail(DomainErrors.AlreadyExists(nameof(Property)), StatusCodes.Status409Conflict);
        }

        var property = command.Model.ToEntity();

        await _propertyRepository.CreateAsync(property, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Property created successfully. PropertyId: {PropertyId}.", property.Id);
        return Result.Success(StatusCodes.Status201Created);
    }
}