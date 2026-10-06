using PropertyRental.Application.PropertyManagement.Models.Unit;

namespace PropertyRental.Application.PropertyManagement.Models;

public record UpdatePropertyModel(
    string Name,
    string Address,
    string RowVersion);