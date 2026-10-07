using PropertyRental.Application.PropertyManagement.Models;
using PropertyRental.Application.PropertyManagement.Models.Property;

namespace PropertyRental.Application.PropertyManagement.Commands.UpdateProperty;

public record UpdatePropertyCommand(Guid Id, UpdatePropertyModel Model);