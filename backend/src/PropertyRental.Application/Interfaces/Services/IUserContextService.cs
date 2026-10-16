using PropertyRental.Application.Common.Models;

namespace PropertyRental.Application.Interfaces.Services;

public interface IUserContextService
{
    UserContext Current { get; }
}