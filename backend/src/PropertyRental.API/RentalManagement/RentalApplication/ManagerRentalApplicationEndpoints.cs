using PropertyRental.API.Authentication;
using PropertyRental.API.Extensions;
using PropertyRental.API.Filters;
using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.Common.Models.Pagination;
using PropertyRental.Application.RentalManagement.Command.ApproveRentalApplication;
using PropertyRental.Application.RentalManagement.Command.DenyRentalApplication;
using PropertyRental.Application.RentalManagement.Command.ReturnRentalApplication;
using PropertyRental.Application.RentalManagement.Models.RentalApplication;
using PropertyRental.Application.RentalManagement.Query.GetAllRentalApplications;
using PropertyRental.Application.RentalManagement.Query.GetRentalApplicationById;

namespace PropertyRental.API.RentalManagement.RentalApplication;

public static class PropertyManagerRentalApplicationEndpoints
{
    private const string PropertyManager = "3.Manager - Rental Application";

    public static void MapManagerRentalApplicationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(RentalApplicationRoutes.PropertyManager.GetAll,
                async (
                    [AsParameters] GetAllRentalApplicationsModel model,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var response = await mediator
                        .ExecuteQueryAsync<
                            GetAllRentalApplicationsQuery,
                            Result<PaginatedModel<RentalApplicationListItemModel>>
                        >(new GetAllRentalApplicationsQuery(model), cancellationToken);

                    return response.ToHttpResult();
                })
            .RequireAuthorization(AuthorizationPolicies.PropertyManager)
            .AddEndpointFilter<ValidationFilter<GetAllRentalApplicationsModel>>()
            .WithTags(PropertyManager);

        app.MapGet(RentalApplicationRoutes.PropertyManager.GetById,
                async (
                    Guid id,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var response = await mediator.ExecuteQueryAsync<
                        GetRentalApplicationByIdQuery,
                        Result<RentalApplicationModel>>
                    (
                        new GetRentalApplicationByIdQuery(id),
                        cancellationToken);

                    return response.ToHttpResult();
                })
            .RequireAuthorization(AuthorizationPolicies.PropertyManager)
            .WithTags(PropertyManager);

        app.MapPost(RentalApplicationRoutes.PropertyManager.Approve,
                async (
                    Guid id,
                    ApproveRentalApplicationModel model,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var response = await mediator.ExecuteCommandAsync<
                        ApproveRentalApplicationCommand,
                        Result>
                    (
                        new ApproveRentalApplicationCommand(id, model),
                        cancellationToken);

                    return response.ToHttpResult();
                })
            .RequireAuthorization(AuthorizationPolicies.PropertyManager)
            .AddEndpointFilter<ValidationFilter<ApproveRentalApplicationModel>>()
            .WithTags(PropertyManager);

        app.MapPost(RentalApplicationRoutes.PropertyManager.Return,
                async (
                    Guid id,
                    RentalApplicationReviewModel model,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var response = await mediator.ExecuteCommandAsync<
                        ReturnRentalApplicationCommand,
                        Result>
                    (
                        new ReturnRentalApplicationCommand(id, model),
                        cancellationToken);

                    return response.ToHttpResult();
                })
            .RequireAuthorization(AuthorizationPolicies.PropertyManager)
            .AddEndpointFilter<ValidationFilter<RentalApplicationReviewModel>>()
            .WithTags(PropertyManager);

        app.MapPost(RentalApplicationRoutes.PropertyManager.Deny,
                async (
                    Guid id,
                    RentalApplicationReviewModel model,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var response = await mediator.ExecuteCommandAsync<
                        DenyRentalApplicationCommand,
                        Result>
                    (
                        new DenyRentalApplicationCommand(id, model),
                        cancellationToken);

                    return response.ToHttpResult();
                })
            .RequireAuthorization(AuthorizationPolicies.PropertyManager)
            .AddEndpointFilter<ValidationFilter<RentalApplicationReviewModel>>()
            .WithTags(PropertyManager);
    }
}