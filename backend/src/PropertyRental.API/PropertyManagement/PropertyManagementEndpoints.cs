using Microsoft.AspNetCore.Mvc;
using PropertyRental.API.Authentication;
using PropertyRental.API.Extensions;
using PropertyRental.API.Filters;
using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.Common.Models.Pagination;
using PropertyRental.Application.PropertyManagement.Commands.CreateProperty;
using PropertyRental.Application.PropertyManagement.Commands.DeleteProperty;
using PropertyRental.Application.PropertyManagement.Commands.UpdateProperty;
using PropertyRental.Application.PropertyManagement.Models;
using PropertyRental.Application.PropertyManagement.Queries.GetAllProperties;
using PropertyRental.Application.PropertyManagement.Queries.GetPropertyById;

namespace PropertyRental.API.PropertyManagement;

public static class PropertyManagementEndpoints
{
    private const string PropertyManagement = "2.Property Management";

    public static void MapPropertyManagementEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(PropertyManagementRoutes.GetAll,
                async (
                    [AsParameters] PaginationParams paginationParams,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var response =
                        await mediator
                            .ExecuteQueryAsync<GetAllPropertiesQuery, Result<PaginatedModel<PropertyListModel>>>(
                                new GetAllPropertiesQuery(paginationParams),
                                cancellationToken);

                    return response.ToHttpResult();
                })
            .RequireAuthorization(AuthorizationPolicies.PropertyManager)
            .WithTags(PropertyManagement);

        app.MapGet(PropertyManagementRoutes.GetById,
                async (
                    Guid id,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var response =
                        await mediator.ExecuteQueryAsync<GetPropertyByIdQuery, Result<PropertyModel>>(
                            new GetPropertyByIdQuery(id), cancellationToken);

                    return response.ToHttpResult();
                })
            .RequireAuthorization(AuthorizationPolicies.PropertyManager)
            .WithTags(PropertyManagement);

        app.MapPost(PropertyManagementRoutes.Create,
                async (
                    CreatePropertyModel model,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var response = await mediator.ExecuteCommandAsync<CreatePropertyCommand, Result>(
                        new CreatePropertyCommand(model), cancellationToken);

                    return response.ToHttpResult();
                })
            .RequireAuthorization(AuthorizationPolicies.PropertyManager)
            .AddEndpointFilter<ValidationFilter<CreatePropertyModel>>()
            .WithTags(PropertyManagement);

        app.MapPut(PropertyManagementRoutes.Update,
                async (
                    UpdatePropertyModel model,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var response = await mediator.ExecuteCommandAsync<UpdatePropertyCommand, Result>(
                        new UpdatePropertyCommand(model), cancellationToken);

                    return response.ToHttpResult();
                })
            .RequireAuthorization(AuthorizationPolicies.PropertyManager)
            .AddEndpointFilter<ValidationFilter<UpdatePropertyModel>>()
            .WithTags(PropertyManagement);

        app.MapDelete(PropertyManagementRoutes.Delete,
                async (
                    [FromBody] DeletePropertyModel model,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var response = await mediator.ExecuteCommandAsync<DeletePropertyCommand, Result>(
                        new DeletePropertyCommand(model), cancellationToken);

                    return response.ToHttpResult();
                })
            .RequireAuthorization(AuthorizationPolicies.PropertyManager)
            .AddEndpointFilter<ValidationFilter<DeletePropertyModel>>()
            .WithTags(PropertyManagement);
    }
}