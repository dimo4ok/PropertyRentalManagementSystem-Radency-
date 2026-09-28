using PropertyRental.Domain.Entities.Enums;

namespace PropertyRental.Application.Common.Models;

public record UserContext(Guid UserId, string UserName, UserRole Role);