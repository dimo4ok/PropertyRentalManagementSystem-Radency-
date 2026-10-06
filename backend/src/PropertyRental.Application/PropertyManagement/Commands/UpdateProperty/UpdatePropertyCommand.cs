using PropertyRental.Application.PropertyManagement.Models;

namespace PropertyRental.Application.PropertyManagement.Commands.UpdateProperty;

public record UpdatePropertyCommand(Guid Id, UpdatePropertyModel Model);