using Microsoft.AspNetCore.Mvc;
using PropertyRental.API.Authentication;
using PropertyRental.API.Extensions;
using PropertyRental.API.Filters;
using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.Common.Models.Pagination;
using PropertyRental.Application.PropertyManagement.Commands.CreateUnit;
using PropertyRental.Application.PropertyManagement.Commands.DeleteUnit;
using PropertyRental.Application.PropertyManagement.Commands.UpdateUnit;
using PropertyRental.Application.PropertyManagement.Models.Unit;
using PropertyRental.Application.PropertyManagement.Queries.GetAllAvailableUnits;

namespace PropertyRental.API.PropertyManagement.Unit;

public static class UnitEndpoints
{
    private const string PropertyManagement = "2.Manager - Unit";
    private const string Applicant = "2.Applicant - Unit";

    public static void MapUnitEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(UnitRoutes.Applicant.GetAvailable,
                async (
                    [AsParameters] PaginationParams paginationParams,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var response = await mediator.ExecuteQueryAsync<
                        GetAvailableUnitsQuery,
                        Result<PaginatedModel<UnitListModel>>>
                    (
                        new GetAvailableUnitsQuery(paginationParams),
                        cancellationToken);

                    return response.ToHttpResult();
                })
            .RequireAuthorization(AuthorizationPolicies.Applicant)
            .AddEndpointFilter<ValidationFilter<PaginationParams>>()
            .WithTags(Applicant);

        app.MapPost(UnitRoutes.PropertyManager.Create,
                async (
                    Guid propertyId,
                    CreateUnitModel model,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var response = await mediator.ExecuteCommandAsync<CreateUnitCommand, Result>(
                        new CreateUnitCommand(propertyId, model), cancellationToken);

                    return response.ToHttpResult();
                })
            .RequireAuthorization(AuthorizationPolicies.PropertyManager)
            .AddEndpointFilter<ValidationFilter<CreateUnitModel>>()
            .WithTags(PropertyManagement);

        app.MapPut(UnitRoutes.PropertyManager.Update,
                async (
                    Guid id,
                    UpdateUnitModel model,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var response = await mediator.ExecuteCommandAsync<UpdateUnitCommand, Result>(
                        new UpdateUnitCommand(id, model), cancellationToken);

                    return response.ToHttpResult();
                })
            .RequireAuthorization(AuthorizationPolicies.PropertyManager)
            .AddEndpointFilter<ValidationFilter<UpdateUnitModel>>()
            .WithTags(PropertyManagement);

        app.MapDelete(UnitRoutes.PropertyManager.Delete,
                async (
                    Guid id,
                    [FromBody] DeleteUnitModel model,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var response = await mediator.ExecuteCommandAsync<DeleteUnitCommand, Result>(
                        new DeleteUnitCommand(id, model), cancellationToken);

                    return response.ToHttpResult();
                })
            .RequireAuthorization(AuthorizationPolicies.PropertyManager)
            .AddEndpointFilter<ValidationFilter<DeleteUnitModel>>()
            .WithTags(PropertyManagement);
    }
}