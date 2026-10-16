using PropertyRental.Application.PropertyManagement.Models;
using PropertyRental.Application.PropertyManagement.Models.Unit;

namespace PropertyRental.Application.PropertyManagement.Commands.UpdateUnit;

public record UpdateUnitCommand(Guid Id, UpdateUnitModel Model);
