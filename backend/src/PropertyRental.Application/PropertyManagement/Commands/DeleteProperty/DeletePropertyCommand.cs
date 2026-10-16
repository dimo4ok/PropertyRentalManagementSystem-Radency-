using PropertyRental.Application.PropertyManagement.Models;
using PropertyRental.Application.PropertyManagement.Models.Property;

namespace PropertyRental.Application.PropertyManagement.Commands.DeleteProperty;

public record DeletePropertyCommand(Guid Id, DeletePropertyModel Model);
