using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PropertyRental.Application.Common.Errors;
using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.Interfaces.Repositories;
using PropertyRental.Application.Interfaces.Services;
using PropertyRental.Application.RentalManagement.Extensions;
using PropertyRental.Application.RentalManagement.Models.RentalApplication;
using PropertyRental.Domain.Entities.RentalEntities;

namespace PropertyRental.Application.RentalManagement.Query.GetApplicantRentalApplicationById;

public class GetApplicantRentalApplicationByIdQueryHandler(
    IRentalApplicationRepository rentalApplicationRepository,
    IUserContextService userContextService,
    ILogger<GetApplicantRentalApplicationByIdQueryHandler> logger
) : IQueryHandler<GetApplicantRentalApplicationByIdQuery, Result<ApplicantRentalApplicationModel>>
{
    private readonly IRentalApplicationRepository _rentalApplicationRepository = rentalApplicationRepository;
    private readonly IUserContextService _userContextService = userContextService;
    private readonly ILogger<GetApplicantRentalApplicationByIdQueryHandler> _logger = logger;

    public async Task<Result<ApplicantRentalApplicationModel>> ExecuteAsync(GetApplicantRentalApplicationByIdQuery query, CancellationToken cancellationToken)
    {
        
        //get for manager and for applicant
        var user = _userContextService.Current;

        _logger.LogInformation(
            "Starting rental application retrieval. UserId: {UserId}, RentalApplicationId: {RentalApplicationId}",
            user.UserId, query.Id);

        var rentalApplication = await _rentalApplicationRepository.GetByIdWithResidencesAsync(query.Id, track: false, cancellationToken);
        if (rentalApplication is null || rentalApplication.UserId != user.UserId)
        {
            _logger.LogWarning(
                "Rental application retrieval failed. Application not found or does not belong to user. UserId: {UserId}, RentalApplicationId: {RentalApplicationId}",
                user.UserId, query.Id);

            return Result<ApplicantRentalApplicationModel>.Fail(DomainErrors.NotFound(nameof(RentalApplication)),
                StatusCodes.Status404NotFound);
        }

        _logger.LogInformation(
            "Rental application retrieved successfully. UserId: {UserId}, RentalApplicationId: {RentalApplicationId}",
            user.UserId, rentalApplication.Id);

        return Result<ApplicantRentalApplicationModel>.Success(rentalApplication.ToApplicantRentalApplicationModel());
    }
}