using PropertyRental.Application.PropertyManagement.Models;
using PropertyRental.Application.PropertyManagement.Models.Unit;

namespace PropertyRental.Application.PropertyManagement.Commands.CreateUnit;

public record CreateUnitCommand(Guid PropertyId, CreateUnitModel Model);
