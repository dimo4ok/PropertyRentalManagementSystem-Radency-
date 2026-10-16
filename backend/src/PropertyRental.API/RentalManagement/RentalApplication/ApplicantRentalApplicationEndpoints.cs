using PropertyRental.API.Authentication;
using PropertyRental.API.Extensions;
using PropertyRental.API.Filters;
using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.Common.Models.Pagination;
using PropertyRental.Application.RentalManagement.Command.CreateRentalApplication;
using PropertyRental.Application.RentalManagement.Command.SubmitRentalApplication;
using PropertyRental.Application.RentalManagement.Command.UpdateRentalApplication;
using PropertyRental.Application.RentalManagement.Command.WithdrawRentalApplication;
using PropertyRental.Application.RentalManagement.Models.RentalApplication;
using PropertyRental.Application.RentalManagement.Query.GetApplicantRentalApplicationById;
using PropertyRental.Application.RentalManagement.Query.GetApplicantRentalApplications;

namespace PropertyRental.API.RentalManagement.RentalApplication;

public static class ApplicantRentalApplicationEndpoints
{
    private const string Applicant = "3.Applicant - Rental Application";

    public static void MapApplicantRentalApplicationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(RentalApplicationRoutes.Applicant.GetAll,
                async (
                    [AsParameters] GetAllRentalApplicationsModel model,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var response = await mediator
                        .ExecuteQueryAsync<
                            GetAllApplicantRentalApplicationsQuery,
                            Result<PaginatedModel<RentalApplicationListItemModel>>
                        >(new GetAllApplicantRentalApplicationsQuery(model), cancellationToken);

                    return response.ToHttpResult();
                })
            .RequireAuthorization(AuthorizationPolicies.Applicant)
            .AddEndpointFilter<ValidationFilter<GetAllRentalApplicationsModel>>()
            .WithTags(Applicant);

        app.MapGet(RentalApplicationRoutes.Applicant.GetById,
                async (
                    Guid id,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var response = await mediator.ExecuteQueryAsync<
                        GetApplicantRentalApplicationByIdQuery,
                        Result<ApplicantRentalApplicationModel>>
                    (
                        new GetApplicantRentalApplicationByIdQuery(id),
                        cancellationToken);

                    return response.ToHttpResult();
                })
            .RequireAuthorization(AuthorizationPolicies.Applicant)
            .WithTags(Applicant);

        app.MapPost(RentalApplicationRoutes.Applicant.Create,
                async (
                    CreateRentalApplicationModel model,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var response =
                        await mediator.ExecuteCommandAsync<CreateRentalApplicationCommand, Result>(
                            new CreateRentalApplicationCommand(model), cancellationToken);

                    return response.ToHttpResult();
                })
            .RequireAuthorization(AuthorizationPolicies.Applicant)
            .AddEndpointFilter<ValidationFilter<CreateRentalApplicationModel>>()
            .WithTags(Applicant);

        app.MapPut(RentalApplicationRoutes.Applicant.Update,
                async (
                    Guid id,
                    UpdateRentalApplicationModel model,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var response = await mediator.ExecuteCommandAsync<
                        UpdateRentalApplicationCommand,
                        Result>
                    (
                        new UpdateRentalApplicationCommand(id, model),
                        cancellationToken);

                    return response.ToHttpResult();
                })
            .RequireAuthorization(AuthorizationPolicies.Applicant)
            .AddEndpointFilter<ValidationFilter<UpdateRentalApplicationModel>>()
            .WithTags(Applicant);

        app.MapPost(RentalApplicationRoutes.Applicant.Submit,
                async (
                    Guid id,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var response = await mediator.ExecuteCommandAsync<
                        SubmitRentalApplicationCommand,
                        Result>
                    (
                        new SubmitRentalApplicationCommand(id),
                        cancellationToken);

                    return response.ToHttpResult();
                })
            .RequireAuthorization(AuthorizationPolicies.Applicant)
            .WithTags(Applicant);

        app.MapPost(RentalApplicationRoutes.Applicant.Withdraw,
                async (
                    Guid id,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var response = await mediator.ExecuteCommandAsync<
                        WithdrawRentalApplicationCommand,
                        Result>
                    (
                        new WithdrawRentalApplicationCommand(id),
                        cancellationToken);

                    return response.ToHttpResult();
                })
            .RequireAuthorization(AuthorizationPolicies.Applicant)
            .WithTags(Applicant);
    }
}