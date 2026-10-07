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
using PropertyRental.Application.PropertyManagement.Models.Property;
using PropertyRental.Application.PropertyManagement.Queries.GetAllProperties;
using PropertyRental.Application.PropertyManagement.Queries.GetPropertyById;

namespace PropertyRental.API.PropertyManagement.Property;

public static class PropertyEndpoints
{
    private const string PropertyManagement = "2.Manager - Property";

    public static void MapPropertyEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(PropertyRoutes.GetAll,
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
            .AddEndpointFilter<ValidationFilter<PaginationParams>>()
            .WithTags(PropertyManagement);

        app.MapGet(PropertyRoutes.GetById,
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

        app.MapPost(PropertyRoutes.Create,
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

        app.MapPut(PropertyRoutes.Update,
                async (
                    Guid id,
                    UpdatePropertyModel model,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var response = await mediator.ExecuteCommandAsync<UpdatePropertyCommand, Result>(
                        new UpdatePropertyCommand(id, model), cancellationToken);

                    return response.ToHttpResult();
                })
            .RequireAuthorization(AuthorizationPolicies.PropertyManager)
            .AddEndpointFilter<ValidationFilter<UpdatePropertyModel>>()
            .WithTags(PropertyManagement);

        app.MapDelete(PropertyRoutes.Delete,
                async (
                    Guid id,
                    [FromBody] DeletePropertyModel model,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var response = await mediator.ExecuteCommandAsync<DeletePropertyCommand, Result>(
                        new DeletePropertyCommand(id, model), cancellationToken);

                    return response.ToHttpResult();
                })
            .RequireAuthorization(AuthorizationPolicies.PropertyManager)
            .AddEndpointFilter<ValidationFilter<DeletePropertyModel>>()
            .WithTags(PropertyManagement);
    }
}