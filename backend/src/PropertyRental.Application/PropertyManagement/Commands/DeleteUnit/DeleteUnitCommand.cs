using PropertyRental.Application.PropertyManagement.Models.Unit;

namespace PropertyRental.Application.PropertyManagement.Commands.DeleteUnit;

public record DeleteUnitCommand(Guid Id, DeleteUnitModel Model);
