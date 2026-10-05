using PropertyRental.Application.Common.Models.Pagination;

namespace PropertyRental.Application.PropertyManagement.Queries.GetAllProperties;

public record GetAllPropertiesQuery(PaginationParams Params);
