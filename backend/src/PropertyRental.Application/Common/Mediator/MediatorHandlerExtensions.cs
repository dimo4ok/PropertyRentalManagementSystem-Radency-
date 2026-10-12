using FitCoachHub.Application.Common.Models;
using Microsoft.Extensions.DependencyInjection;
using PropertyRental.Application.Authentification.Commands.SignIn;
using PropertyRental.Application.Authentification.Commands.SignUp;
using PropertyRental.Application.Authentification.Models;
using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.Common.Models.Pagination;
using PropertyRental.Application.PropertyManagement.Commands.CreateProperty;
using PropertyRental.Application.PropertyManagement.Commands.CreateUnit;
using PropertyRental.Application.PropertyManagement.Commands.DeleteProperty;
using PropertyRental.Application.PropertyManagement.Commands.DeleteUnit;
using PropertyRental.Application.PropertyManagement.Commands.UpdateProperty;
using PropertyRental.Application.PropertyManagement.Commands.UpdateUnit;
using PropertyRental.Application.PropertyManagement.Models;
using PropertyRental.Application.PropertyManagement.Models.Property;
using PropertyRental.Application.PropertyManagement.Models.Unit;
using PropertyRental.Application.PropertyManagement.Queries.GetAllAvailableUnits;
using PropertyRental.Application.PropertyManagement.Queries.GetAllProperties;
using PropertyRental.Application.PropertyManagement.Queries.GetPropertyById;
using PropertyRental.Application.RentalManagement.Command.ApproveRentalApplication;
using PropertyRental.Application.RentalManagement.Command.CreateRentalApplication;
using PropertyRental.Application.RentalManagement.Command.DenyRentalApplication;
using PropertyRental.Application.RentalManagement.Command.ReturnRentalApplication;
using PropertyRental.Application.RentalManagement.Command.SubmitRentalApplication;
using PropertyRental.Application.RentalManagement.Command.UpdateRentalApplication;
using PropertyRental.Application.RentalManagement.Command.WithdrawRentalApplication;
using PropertyRental.Application.RentalManagement.Models.RentalApplication;
using PropertyRental.Application.RentalManagement.Query.GetAllRentalApplications;
using PropertyRental.Application.RentalManagement.Query.GetApplicantRentalApplicationById;
using PropertyRental.Application.RentalManagement.Query.GetApplicantRentalApplications;
using PropertyRental.Application.RentalManagement.Query.GetRentalApplicationById;

namespace PropertyRental.Application.Common.Mediator;

public static class MediatorHandlerExtensions
{
    public static IServiceCollection AddMediatorHandlers(this IServiceCollection services)
    {
        //auth
        services.AddScoped<ICommandHandler<SignUpCommand, Result<AuthResponse>>, SignUpCommandHandler>();
        services.AddScoped<ICommandHandler<SignInCommand, Result<AuthResponse>>, SignInCommandHandler>();

        //Property
        services.AddScoped<ICommandHandler<CreatePropertyCommand, Result>, CreatePropertyCommandHandler>();
        services.AddScoped<ICommandHandler<UpdatePropertyCommand, Result>, UpdatePropertyCommandHandler>();
        services.AddScoped<ICommandHandler<DeletePropertyCommand, Result>, DeletePropertyCommandHandler>();

        services.AddScoped<IQueryHandler<GetPropertyByIdQuery, Result<PropertyModel>>, GetPropertyByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetAllPropertiesQuery, Result<PaginatedModel<PropertyListModel>>>,
            GetAllPropertiesQueryHandler>();

        //Unit
        services.AddScoped<ICommandHandler<CreateUnitCommand, Result>, CreateUnitCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateUnitCommand, Result>, UpdateUnitCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteUnitCommand, Result>, DeleteUnitCommandHandler>();

        services.AddScoped<IQueryHandler<GetAvailableUnitsQuery, Result<PaginatedModel<UnitListModel>>>,
            GetAvailableUnitsQueryHandler>();

        //Rental
        services
            .AddScoped<ICommandHandler<CreateRentalApplicationCommand, Result>,
                CreateRentalApplicationCommandHandler>();

        services
            .AddScoped<ICommandHandler<UpdateRentalApplicationCommand, Result>,
                UpdateRentalApplicationCommandHandler>();

        services
            .AddScoped<ICommandHandler<SubmitRentalApplicationCommand, Result>,
                SubmitRentalApplicationCommandHandler>();

        services
            .AddScoped<ICommandHandler<WithdrawRentalApplicationCommand, Result>,
                WithdrawRentalApplicationCommandHandler>();

        services
            .AddScoped<ICommandHandler<ApproveRentalApplicationCommand, Result>,
                ApproveRentalApplicationCommandHandler>();

        services
            .AddScoped<ICommandHandler<ReturnRentalApplicationCommand, Result>,
                ReturnRentalApplicationCommandHandler>();

        services
            .AddScoped<ICommandHandler<DenyRentalApplicationCommand, Result>, DenyRentalApplicationCommandHandler>();

        services
            .AddScoped<IQueryHandler<GetApplicantRentalApplicationByIdQuery, Result<ApplicantRentalApplicationModel>>,
                GetApplicantRentalApplicationByIdQueryHandler>();

        services
            .AddScoped<IQueryHandler<GetRentalApplicationByIdQuery, Result<RentalApplicationModel>>,
                GetRentalApplicationByIdQueryHandler>();
        
        services
            .AddScoped<
                IQueryHandler<
                    GetAllApplicantRentalApplicationsQuery,
                    Result<PaginatedModel<RentalApplicationListItemModel>>>,
                GetAllApplicantRentalApplicationsQueryHandler>();

        services
            .AddScoped<
                IQueryHandler<
                    GetAllRentalApplicationsQuery,
                    Result<PaginatedModel<RentalApplicationListItemModel>>>,
                GetAllRentalApplicationsQueryHandler>();

        return services;
    }
}