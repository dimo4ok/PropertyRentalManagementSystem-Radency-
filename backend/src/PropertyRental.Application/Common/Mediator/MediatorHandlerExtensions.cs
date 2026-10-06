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
using PropertyRental.Application.PropertyManagement.Queries.GetAllProperties;
using PropertyRental.Application.PropertyManagement.Queries.GetPropertyById;

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
        
        return services;
    }
}