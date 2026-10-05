using PropertyRental.Application.Common.Models;
using PropertyRental.Application.PropertyManagement.Models;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.PropertyManagement.Services;

public interface IUnitService
{
    public Task<Result> UpdateAsync(
        Property property,
        ICollection<UpdateUnitModel> models,
        CancellationToken cancellationToken);
}