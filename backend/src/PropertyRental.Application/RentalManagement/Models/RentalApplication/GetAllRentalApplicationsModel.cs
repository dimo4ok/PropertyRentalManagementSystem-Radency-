using PropertyRental.Application.Common.Models.Pagination;
using PropertyRental.Domain.Entities.Enums;

namespace PropertyRental.Application.RentalManagement.Models.RentalApplication;

public record GetAllRentalApplicationsModel(
    ApplicationStatus? Status,
    Guid? PropertyId,
    int PageNumber,
    int PageSize);