using PropertyRental.Application.Common.Models;

namespace PropertyRental.Application.Interfaces;

public interface IUserContextService
{
    UserContext Current { get; }
}