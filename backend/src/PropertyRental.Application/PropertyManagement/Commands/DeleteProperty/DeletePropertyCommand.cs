using PropertyRental.Application.PropertyManagement.Models;

namespace PropertyRental.Application.PropertyManagement.Commands.DeleteProperty;

public record DeletePropertyCommand(Guid Id, DeletePropertyModel Model);
