using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PropertyRental.Application.Authentification.Interfaces;
using PropertyRental.Application.Common.Configuration;
using PropertyRental.Application.Interfaces;
using PropertyRental.Application.PropertyManagement.Interfaces;
using PropertyRental.Application.RentalManagement.Interfaces;
using PropertyRental.Domain.Entities.Identity;
using PropertyRental.Infrastructure.Authentication;
using PropertyRental.Infrastructure.Persistence;
using PropertyRental.Infrastructure.Persistence.Seeding;
using PropertyRental.Infrastructure.PropertyManagement;
using PropertyRental.Infrastructure.RentalManagement;
using PropertyRental.Infrastructure.Services;

namespace PropertyRental.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddIdentityCore<User>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>();

        // Options
        services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

        //Authentication + JWT
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtOptions:Issuer"],
                    ValidAudience = configuration["JwtOptions:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes(configuration["JwtOptions:SecretKey"]!))
                };
            });

        services.AddScoped<DbSeeder>();
        services.AddScoped<IUserContextService, UserContextService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<IPropertyRepository, PropertyRepository>();
        services.AddScoped<IUnitRepository, UnitRepository>();
        services.AddScoped<IUnitTypeRepository, UnitTypeRepository>();
        services.AddScoped<IRentalApplicationRepository, RentalApplicationRepository>();

        services.AddHttpContextAccessor();

        return services;
    }
}